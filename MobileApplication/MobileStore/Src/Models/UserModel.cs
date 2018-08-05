﻿namespace MobileStore.Src.Models
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
        public UserModel(string name, string surename, string cellphone, string address, string login, string password, string email, int roleid)
        {
            this.RoleId = roleid;
            this.Name = name;
            this.Surname = surename;
            this.CellPhone = cellphone;
            this.Address = address;
            this.Login = login;
            this.Password = password;
            this.Email = email;
        }
    }
}
