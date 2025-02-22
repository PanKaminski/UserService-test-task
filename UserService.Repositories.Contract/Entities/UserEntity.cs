namespace UserService.Repositories.Contract.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
            
        }
        public UserEntity(string name, string email, string passwordHash, int role)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Role { get; set; }
    }
}
