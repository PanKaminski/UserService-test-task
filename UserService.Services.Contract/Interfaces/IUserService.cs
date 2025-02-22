using UserService.Services.Contract.Models.Requests;
using UserService.Services.Contract.Models.Responses;

namespace UserService.Services.Contract.Interfaces
{
    public interface IUserService
    {
        ServerOperationResult CreateUser(CreateUserRequest model);
        Task<UsersResponse> GetUsersAsync();
        ServerOperationResult UpdateUserRole(UpdateUserRoleRequest model);
    }
}
