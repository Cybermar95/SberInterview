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

        // ��� ���������� user ��� �������� �� ������������, SingleAsync ����� ����������� ������.
        // ������� await ��� �� �������� ��������� � �������� 1 ���������.
        public async Task<User> GetUserByLoginAsync(string login) => await _context.Users.SingleAsync(u => u.Login == login);
    }
}