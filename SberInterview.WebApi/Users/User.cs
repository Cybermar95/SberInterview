using System.Collections.Generic;
using SberInterview.WebApi.Accounts;

namespace SberInterview.WebApi.Users
{
    // Отсутсвует логика в местоположении класса в solution explorer, возможно стоит создать папку Model и расположить класс в ней (как и класс Account).
    public class User
    {
        public string Login { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Циклическая ссылка.
        public ICollection<Account> Accounts { get; set; }
    }
}