using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class SellerCreateModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public SellerCreateModel()
        {

        }
        public SellerCreateModel(string name, string address,string cellphone, string login, string email, string password)
        {
            this.Name = name;
            this.Address = address;
            this.CellPhone = cellphone;
            this.Login = login;
            this.Email = email;
            this.Password = password;
        }
    }
}
