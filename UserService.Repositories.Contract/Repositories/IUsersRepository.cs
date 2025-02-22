using UserService.Repositories.Contract.Entities;

namespace UserService.Repositories.Contract.Repositories
{
    public interface IUsersRepository
    {
        Task<ICollection<UserEntity>> GetAllAsync();
        bool DoesExistByEmail(string userEmail);
        void Add(UserEntity user);
        void UpdateRole(int userId, int roleCode);
    }
}
