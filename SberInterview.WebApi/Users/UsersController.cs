using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SberInterview.WebApi.Accounts;
using SberInterview.WebApi.Users.Create;
using SberInterview.WebApi.Users.Get;

// ����� �� �������� ����������� ControllerBase.
// �������������� ������ �� ���������� �� ������ ��������� ��������, ������� ������������� ActionResult<T>.
// � ������ ������������� ������, ������������ ����� ������ ��� ������, ���������� ��������� ������� � ���� try/catch.

// AddAccountToUserAsync �������� ������������� ���� (�������� ��������� � ��),
// ���������� �������� ����� ���������� � ������ �� ������ ��������� SberDbContext

// ������������� [Authorize] ������ � GetUserByLoginAsync ������ ����� ������.

namespace SberInterview.WebApi.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SberDbContext _context;

        // ���������� ��������� ����� ������������.
        public UsersController(IMediator mediator, SberDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // HttpGet("{login}") �������n login �� URL, � �� �� FromBody. �.�. ��� Get ������, ��������� ��������� login �� url.
        // �������� ������� ������������� [Authorize] ������ � ���� ������.
        [Authorize]
        [HttpGet("{login}")]
        public async Task<ActionResult<User>> GetUserByLoginAsync(string login)
        {
            ActionResult<User> result;
            try
            {
                result = await _mediator.Send(new GetUserByLoginCommand { Login = login });
            }
            catch
            {
                result = Conflict();
            }
            return result;
        }

        // �������� �� �������� ���� Post �������.
        // ��������� ������� �� FromQuery ������, ���������� FromBody.
        // (������ '/' � ��������)
        // ������� �������� �� ��������� � �������� api/Users. create ����������.
        [HttpPost("create")]
        public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserCommand command)
        {
            ActionResult result;
            try
            {
                await _mediator.Send(command);
                result = Ok();
            }
            catch
            {
                result = Conflict();
            }
            return result;
        }

        // �� ���� ��� post � �� get ������.
        // ���������� �������� � api/Users �����������, �������� ����� ������� ��������� ���������� ��� ������ � ����������.
        // ������� �������� login ������ �� �������.
        // ��������� ������� ������ ������ � �������������� � �� � AddAccountToUserHandler. �������������� � �� � ���� ������/���� �������� �������.
        [HttpPost("addAccount")]
        public async Task<ActionResult> AddAccountToUserAsync([FromBody] string login)
        {
            ActionResult result;

            try
            {
                // ����������� FirstOrDefault ��� �������� �� null.
                // ������������� �� � ���� �����������!.
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (user is not null)
                {
                    // ������������������ user.Accounts ������ ��� ������ ����������, ���������� �������� ��������� �� null.
                    var newAccount = new Account { UserLogin = user.Login };

                    if (user.Accounts is not null)
                    {
                        user.Accounts.Add(newAccount);
                    }
                    else
                    {
                        user.Accounts = new List<Account>() { newAccount };
                    }
                    _context.SaveChangesAsync().Wait();
                    result = Ok();
                }
                else
                {
                    result = NotFound();
                }
            }
            catch
            {
                result = Conflict();
            }

            return result;
        }
    }
}