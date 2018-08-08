using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string ActivationCode { get; set; }
        public UserModel(string name, string surname, string cellphone, string address, string login, string password, string email, int roleid)
        {
            this.RoleId = roleid;
            this.Name = name;
            this.Surname = surname;
            this.CellPhone = cellphone;
            this.Address = address;
            this.Login = login;
            this.Password = password;
            this.Email = email;
        }
    }
}
