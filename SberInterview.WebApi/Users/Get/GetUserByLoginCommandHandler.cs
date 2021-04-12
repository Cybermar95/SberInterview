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

        // �������� ������ ��������� ILoggerFactory ����� �������� ����� ILogger<GetUserByLoginCommandHandler>, ��� �� �� �������� ��� � ������������.
        public GetUserByLoginCommandHandler(UsersRepository usersRepository, ILoggerFactory loggerFactory)
        {
            _usersRepository = usersRepository;
            _logger = loggerFactory.CreateLogger<GetUserByLoginCommandHandler>();
        }

        // ����� �������� � 1 return.
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

            // ����������� null ������ ����� �� ���������, ��������� ����� ������������ � MediatR.
            return user;
        }
    }
}