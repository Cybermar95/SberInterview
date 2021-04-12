using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SberInterview.WebApi.Users.Get
{
    public class GetUserByLoginCommandHandler : IRequestHandler<GetUserByLoginCommand, User>
    {
        private readonly UsersRepository _usersRepository;
        private readonly ILogger<GetUserByLoginCommandHandler> _logger;

        public GetUserByLoginCommandHandler(UsersRepository usersRepository, ILoggerFactory loggerFactory)
        {
            _usersRepository = usersRepository;
            _logger = loggerFactory.CreateLogger<GetUserByLoginCommandHandler>();
        }

        public async Task<User> Handle(GetUserByLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _usersRepository.GetUserByLoginAsync(request.Login).Result;
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return null;
        }
    }
}