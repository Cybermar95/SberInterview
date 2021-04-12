using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SberInterview.WebApi.Users
{
    public class UsersRepository
    {
        private readonly SberDbContext _context;

        public UsersRepository(SberDbContext context)
        {
            _context = context;
        }

        // ѕри добавлении user нет проверку на уникальность, SingleAsync может выбрасывать ошибки.
        // ƒобавил await что бы избежать обращени€ в пределах 1 контекста.
        public async Task<User> GetUserByLoginAsync(string login) => await _context.Users.SingleAsync(u => u.Login == login);
    }
}