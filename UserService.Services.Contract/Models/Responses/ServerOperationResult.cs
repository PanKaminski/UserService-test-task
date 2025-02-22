using UserService.Services.Contract.Enums;

namespace UserService.Services.Contract.Models.Responses
{
    public class ServerOperationResult
    {
        public ServerOperationResult(bool isSuccess, string message, ServerResultCode code = ServerResultCode.None)
        {
            IsSuccess = isSuccess;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Code = code;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ServerResultCode Code { get; set; }

        public static ServerOperationResult Failed(string message, ServerResultCode code) => new ServerOperationResult(false, message, code);
    }
}
