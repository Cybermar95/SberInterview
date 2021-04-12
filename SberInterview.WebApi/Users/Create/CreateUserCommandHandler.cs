using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SberInterview.WebApi.Users.Create;

// ReSharper disable All

namespace SberInterview.WebApi.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly SberDbContext _context;
        // Отсутсвует инжекция зависимости IMapper.
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(SberDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // Нет проверки на то что данный user уже существует.
            _context.Add(_mapper.Map<User>(command));
            _context.SaveChanges();

            return Unit.Value;
        }
    }
}