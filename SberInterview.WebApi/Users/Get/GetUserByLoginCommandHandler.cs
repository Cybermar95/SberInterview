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

        // Возможно вместо внедрения ILoggerFactory стоит внедрить сразу ILogger<GetUserByLoginCommandHandler>, что бы не собирать его в конструкторе.
        public GetUserByLoginCommandHandler(UsersRepository usersRepository, ILoggerFactory loggerFactory)
        {
            _usersRepository = usersRepository;
            _logger = loggerFactory.CreateLogger<GetUserByLoginCommandHandler>();
        }

        // Лучше привести к 1 return.
        public async Task<User> Handle(GetUserByLoginCommand request, CancellationToken cancellationToken)
        {
            User user = default;
            try
            {
                user = _usersRepository.GetUserByLoginAsync(request.Login).Result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            // Возвращение null скорее всего не правильно, требуется лучше ознакомиться с MediatR.
            return user;
        }
    }
}