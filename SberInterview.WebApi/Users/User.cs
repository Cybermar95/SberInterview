using System.Collections.Generic;
using SberInterview.WebApi.Accounts;

namespace SberInterview.WebApi.Users
{
    public class User
    {
        public string Login { get; set; }
        
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public ICollection<Account> Accounts { get; set; }
    }
}