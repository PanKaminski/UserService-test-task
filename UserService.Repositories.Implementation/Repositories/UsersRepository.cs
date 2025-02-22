using Microsoft.Data.SqlClient;
using UserService.Repositories.Contract.Entities;
using UserService.Repositories.Contract.Repositories;

namespace UserService.Repositories.Implementation.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly string connectionString;

        public UsersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(UserEntity user)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES (@Name, @Email, @PasswordHash, @Role)", connection))
            {
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Role", user.Role);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool DoesExistByEmail(string userEmail)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", userEmail);

                connection.Open();
                var result = command.ExecuteScalar();
                return (int)result > 0;
            }
        }

        public async Task<ICollection<UserEntity>> GetAllAsync()
        {
            var users = new List<UserEntity>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT Id, Name, Email, Role FROM Users", connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(ReadUser(reader));
                    }
                }
            }

            return users;
        }
        public void UpdateRole(int userId, int roleCode)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("UPDATE Users SET Role = @Role WHERE Id = @UserId", connection))
            {
                command.Parameters.AddWithValue("@Role", roleCode);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        private UserEntity ReadUser(SqlDataReader reader) => new UserEntity
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Role = reader.GetInt32(3),
        };
    }
}
