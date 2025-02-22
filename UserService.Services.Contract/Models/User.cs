using System.Text.Json.Serialization;
using UserService.Services.Contract.Enums;

namespace UserService.Services.Contract.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        public UserRoleCode Role { get; set; }
    }
}
