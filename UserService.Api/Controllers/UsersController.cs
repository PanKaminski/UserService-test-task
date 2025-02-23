using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using UserService.Services.Contract.Models.Requests;
using UserService.Services.Contract.Interfaces;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService usersService;

        public UsersController(IUserService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost("create")]
        public IActionResult Create(CreateUserRequest request)
        {
            return Ok(usersService.CreateUser(request));
        }

        [HttpPut("updateRole")]
        public IActionResult UpdateRole(UpdateUserRoleRequest request)
        {
            return Ok(usersService.UpdateUserRole(request));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await usersService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await ReceiveUpdates(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task ReceiveUpdates(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                try
                {
                    var updateRequest = JsonSerializer.Deserialize<UpdateUserRequest>(message);
                    if (updateRequest is not null)
                    {
                        var updateResult = await usersService.UpdateUserAsync(updateRequest);
                        var resultBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(updateResult));
                        await webSocket.SendAsync(resultBytes, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        await webSocket.SendAsync(Encoding.UTF8.GetBytes("Invalid user data."), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                catch (JsonException)
                {
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes("Error processing user data."), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
