using UserService.Services.Contract.Enums;

namespace UserService.Services.Contract.Models.Requests
{
    public class UpdateUserRoleRequest
    {
        public int Id { get; set; }
        public UserRoleCode Role { get; set; }
    }
}
