using UserService.Repositories.Contract.Entities;
using UserService.Repositories.Contract.Repositories;
using UserService.Services.Contract.Enums;
using UserService.Services.Contract.Interfaces;
using UserService.Services.Contract.Models.Requests;
using UserService.Services.Contract.Models.Responses;
using UserService.Services.Implementation.Utils;

namespace UserService.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public ServerOperationResult CreateUser(CreateUserRequest model)
        {
            if (UserValidator.IsValidEmail(model.Email))
                return ServerOperationResult.Failed("Email is invalid", ServerResultCode.InvalidEmail);
            if (UserValidator.IsValidName(model.Name))
                return ServerOperationResult.Failed("User name is required", ServerResultCode.UserNameIsRequired);
            if (UserValidator.IsValidPassword(model.Password))
                return ServerOperationResult.Failed("Password is invalid", ServerResultCode.InvalidPassword);
            if (UserValidator.IsValidRole(model.Role))
                return ServerOperationResult.Failed("Invalid role is selected", ServerResultCode.InvalidRoleSelected);
            
            if (usersRepository.DoesExistByEmail(model.Email))
                return ServerOperationResult.Failed("User with such email already exists", ServerResultCode.UserWithSuchEmailExists);
            
            var useEntity = new UserEntity(
                model.Name, model.Email, BCrypt.Net.BCrypt.HashPassword(model.Password), (int)model.Role);

            usersRepository.Add(useEntity);

            return new ServerOperationResult(true, "User has been created");
        }

        public async Task<UsersResponse> GetUsersAsync()
        {
            var users = (await usersRepository.GetAllAsync()).Select(u => u.ToModel()).ToList();

            return new UsersResponse { Users = users };
        }

        public async Task<ServerOperationResult> UpdateUserAsync(UpdateUserRequest model)
        {
            if (UserValidator.IsValidEmail(model.Email))
                return ServerOperationResult.Failed("Email is invalid", ServerResultCode.InvalidEmail);
            if (UserValidator.IsValidName(model.Name))
                return ServerOperationResult.Failed("User name is required", ServerResultCode.UserNameIsRequired);

            var existingUser = await usersRepository.GetUserByIdAsync(model.Id);
            if (existingUser is null)
            {
                return ServerOperationResult.Failed($"User with the id {model.Id} does not exist", ServerResultCode.UserWithSuchIdDoesNotExist);
            }

            existingUser.Email = model.Email;
            existingUser.Name = model.Name;

            await usersRepository.UpdateUserAsync(existingUser);

            return new ServerOperationResult(true, $"User {model.Name} has been updated");
        }

        public ServerOperationResult UpdateUserRole(UpdateUserRoleRequest model)
        {
            if (UserValidator.IsValidRole(model.Role))
                return ServerOperationResult.Failed("Invalid role is selected", ServerResultCode.InvalidRoleSelected);

            usersRepository.UpdateRole(model.Id, (int)model.Role);

            return new ServerOperationResult(true, "User role has been updated");
        }
    }
}