namespace UserService.Services.Contract.Models.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
