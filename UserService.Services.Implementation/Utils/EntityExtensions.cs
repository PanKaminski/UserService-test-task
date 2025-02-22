using UserService.Repositories.Contract.Entities;
using UserService.Services.Contract.Enums;
using UserService.Services.Contract.Models;

namespace UserService.Services.Implementation.Utils
{
    public static class EntityExtensions
    {
        public static User ToModel(this UserEntity entity) => new User
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Role = (UserRoleCode)entity.Role,
        };
    }
}
