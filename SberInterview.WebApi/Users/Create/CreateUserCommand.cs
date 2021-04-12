using MediatR;

namespace SberInterview.WebApi.Users.Create
{
    public class CreateUserCommand : IRequest<Unit>
    {
        public string Login { get; set; }
        
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}