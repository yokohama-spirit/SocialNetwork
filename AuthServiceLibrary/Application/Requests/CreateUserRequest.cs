using MediatR;

namespace AuthServiceLibrary.Application.Requests
{
    public class CreateUserRequest : IRequest<string>
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
