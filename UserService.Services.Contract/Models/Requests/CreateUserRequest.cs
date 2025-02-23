using UserService.Services.Contract.Enums;

namespace UserService.Services.Contract.Models.Requests
{
    public class CreateUserRequest : UpdateUserRequest
    {
        public string Password { get; set; }
        public UserRoleCode Role { get; set; }
    }
}
