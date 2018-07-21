using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class CustomerModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public CustomerModel()
        {

        }
        public CustomerModel(string name, string surname, string login, string email, string password)
        {
            this.Name = name;
            this.Surname = surname;
            this.Login = login;
            this.Email = email;
            this.Password = password;
        }
    }
}
