using SberInterview.WebApi.Users;

namespace SberInterview.WebApi.Accounts
{
    public class Account
    {
        public int Id { get; set; }

        public double Balance { get; set; }

        // Циклическая ссылка, могут быть проблеммы при использовании сериализации.
        public User User { get; set; }

        // Дублирование информации, Login содержится в свойстве User.
        public string UserLogin { get; set; }
    }
}