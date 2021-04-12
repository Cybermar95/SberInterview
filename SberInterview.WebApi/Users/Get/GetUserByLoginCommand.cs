using MediatR;

namespace SberInterview.WebApi.Users.Get
{
    public class GetUserByLoginCommand : IRequest<User>
    {
        public string Login { get; set; }
    }
}