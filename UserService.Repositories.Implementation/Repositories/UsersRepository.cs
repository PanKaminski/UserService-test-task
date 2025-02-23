using Microsoft.Data.SqlClient;
using UserService.Repositories.Contract.Entities;
using UserService.Repositories.Contract.Repositories;

namespace UserService.Repositories.Implementation.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private const string InsertUserQuery = "INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES (@Name, @Email, @PasswordHash, @Role)";
        private const string GetByEmailQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
        private const string GetAllUsersQuery = "SELECT Id, Name, Email, Role FROM Users";
        private const string UpdateRoleQuery = "UPDATE Users SET Role = @Role WHERE Id = @UserId";
        private const string GetByIdQuery = "SELECT Id, Name, Email, Role FROM Users WHERE Id = @Id";
        private const string UpdateByIdQuery = "UPDATE Users SET Name = @Name, Email = @Email, Role = @Role WHERE Id = @Id";
        private readonly string connectionString;

        public UsersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(UserEntity user)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(InsertUserQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Role", user.Role);

                connection.OpenAsync();
                command.ExecuteNonQueryAsync();
            }
        }

        public bool DoesExistByEmail(string userEmail)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(GetByEmailQuery, connection))
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
            using (var command = new SqlCommand(GetAllUsersQuery, connection))
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
            using (var command = new SqlCommand(UpdateRoleQuery, connection))
            {
                command.Parameters.AddWithValue("@Role", roleCode);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(GetByIdQuery, connection);
            command.Parameters.AddWithValue("@Id", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return ReadUser(reader);
            }

            return null;
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(UpdateByIdQuery, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", user.Role);

            await command.ExecuteNonQueryAsync();
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
