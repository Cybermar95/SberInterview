using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SberInterview.WebApi.Accounts;
using SberInterview.WebApi.Users.Create;
using SberInterview.WebApi.Users.Get;

// ReSharper disable All

namespace SberInterview.WebApi.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly IMediator _mediator;
        private readonly SberDbContext _context;

        public UsersController(IMediator mediator, SberDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [Authorize]
        [HttpGet("{login}")]
        public async Task<User> GetUserByLoginAsync([FromBody] string login)
        {
            return await _mediator.Send(new GetUserByLoginCommand
            {
                Login = login
            });
        }

        [HttpPost("/createUser")]
        public async Task GetUserByLoginAsync([FromQuery] CreateUserCommand command)
        {
            await _mediator.Send(command);
        }
        
        [HttpGet("add")]
        public async Task AddAccountToUserAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            user.Accounts = new List<Account>
            {
                new Account
                {
                    UserLogin = user.Login
                }
            };

            _context.SaveChangesAsync().Wait();
        }
    }
}