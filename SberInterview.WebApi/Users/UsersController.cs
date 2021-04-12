using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SberInterview.WebApi.Accounts;
using SberInterview.WebApi.Users.Create;
using SberInterview.WebApi.Users.Get;

// Класс не является наследником ControllerBase.
// Представленные методы не возвращают на клиент результат операции, добавил использование ActionResult<T>.
// В случае возникновения ошибок, потенциально может упасть вся служба, добавленно временное решение в виде try/catch.

// AddAccountToUserAsync нарушает использование слоёв (напрямую обращется к бд),
// необходимо добавить новый обработчик и убрать из класса внедрение SberDbContext

// Использование [Authorize] только в GetUserByLoginAsync скорее всего ошибка.

namespace SberInterview.WebApi.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SberDbContext _context;

        // Отсутсвует внедрение обоих зависимостей.
        public UsersController(IMediator mediator, SberDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // HttpGet("{login}") получаеn login из URL, а не из FromBody. Т.к. это Get запрос, оставляем получение login из url.
        // Вызывает вопросы использование [Authorize] только в этом методе.
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

        // Название не отражает суть Post запроса.
        // Получение объекта из FromQuery ошибка, необходимо FromBody.
        // (лишний '/' в роутинге)
        // Спорное название мы находимся в роутинге api/Users. create достаточно.
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

        // По сути это post а не get запрос.
        // Добавление аккаунта в api/Users сомнительно, возможно стоит создать отдельный контроллер для работы с аккаунтами.
        // Входной параметр login берётся из воздуха.
        // Требуется вынести бизнес логику и взаимодействие с бд в AddAccountToUserHandler. Взаимодействие с бд в этом классе/слое является ошибкой.
        [HttpPost("addAccount")]
        public async Task<ActionResult> AddAccountToUserAsync([FromBody] string login)
        {
            ActionResult result;

            try
            {
                // Использован FirstOrDefault без проверки на null.
                // Использование бд в слое контроллера!.
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (user is not null)
                {
                    // Переинициализируем user.Accounts каждый раз вместо добавления, пропущенна проверка коллекции на null.
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