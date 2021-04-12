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
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(SberDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _context.Add(_mapper.Map<User>(command));
            _context.SaveChanges();
            
            return Unit.Value;
        }
    }
}