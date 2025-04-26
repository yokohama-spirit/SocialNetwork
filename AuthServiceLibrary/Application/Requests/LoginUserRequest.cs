using MediatR;

namespace AuthServiceLibrary.Application.Requests
{
    public class LoginUserRequest : IRequest<string>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
