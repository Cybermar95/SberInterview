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

        public Task<User> GetUserByLoginAsync(string login)
        {
            return _context.Users.SingleAsync(u => u.Login == login);
        }
    }
}