using UserService.Repositories.Contract.Repositories;
using UserService.Repositories.Implementation.Repositories;
using UserService.Services.Contract.Interfaces;
using AppUserService = UserService.Services.Implementation.UserService;
namespace UserService.Api.Extensions
{
    public static class AppBuilderExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddSingleton<IUserService, AppUserService>();

            builder.Services.AddSingleton<IUsersRepository>(_ => new UsersRepository(connectionString));

            return builder;
        }
    }
}
