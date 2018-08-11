
#define APISIM
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.Graphics;
using MobileApplication.Src.Models;
using Newtonsoft.Json;

namespace MobileApplication.Src.API
{
    public static class UserAPIConection
    {
        /// <summary>
        /// User name, User pair
        /// </summary>
        private static Dictionary<string, UserModel> UserDataBase;
        /// <summary>
        /// Seller id seller pair
        /// </summary>
        private static Dictionary<int, Seller> SellerDataBase;
        public static UserModel User { get; private set; }

        private static readonly string ulr = string.Format("http://134.86.19.105:5003/api/SellerProduct/products/", 8);

        static UserAPIConection()
        {
            var ran = new Random();
            var images = new List<string>() {
                @"https://images.samsung.com/is/image/samsung/p5/ru/smartphones/PCD_Purple.png?$ORIGIN_PNG$",
                @"https://drop.ndtv.com/TECH/product_database/images/6142017123101PM_635_htc_desire_628_dual_sIM.jpeg",
                @"https://akket.com/wp-content/uploads/2018/04/Nokia-9-42.jpg",
                @"https://3dnews.ru/assets/external/illustrations/2018/03/16/967085/sm.x1.750.jpg"
            };
            var Products = new List<Product>();
            var brannds = ProductAPIConection.GetBrands();
            foreach (var item in items)
            {
                var product = new Product()
                {
                    Name = brannds[ran.Next(0, 4)] + " " + item,
                    Price = ran.Next(1000, 10000),
                    //Image = images[ran.Next(0, 4)],
                    Quantity = 5,
                    Brand = item
                };
                Products.Add(product);
            }

            UserDataBase = new Dictionary<string, UserModel>();
            UserDataBase.Add("Argishty", new UserModel("Argishty", "Ayavzyan", "738274928", "Yerevan", "argishta", "12345", "aps.gmail.com", 1));
            SellerDataBase = new Dictionary<int, Seller>();
            var index = 0;
            for (var i = 1; i < 5; ++i)
            {
                var seller = new Seller()
                {
                    Id = i,
                    Name = $"Mobile Centr{i}",
                    Address = $"Yerevan{i}",
                    Rating = 3.4m,
                    CellPhone = "+(374) 94047454",
                    Email = "aps@mail.ru"
                };


                SellerDataBase.Add(i, seller);
                ProductAPIConection.AddProduct(seller.Id, Products[index++]);
                ProductAPIConection.AddProduct(seller.Id, Products[index++]);
                ProductAPIConection.AddProduct(seller.Id, Products[index++]);
            }
        }

        public static bool LogOut()
        {
            if (User == null)
            {
                return false;
            }
            User = null;

            return true;
        }

        public static bool SessionActivity() => (User != null) ? true : false;

        public static bool SigeIn(string Login, string password)
        {
            if (!UserDataBase.ContainsKey(Login) || UserDataBase[Login].Password != password)
            {
                return false;
            }

            User = UserDataBase[Login];
            return true;
        }

        public static bool RegisterCustomer(string name, string surname, string login, string email, string password)
        {
            if (UserDataBase.ContainsKey(login))
            {
                return false;
            }

            UserDataBase.Add(login, new UserModel(name, surname, null, null, login, password, email, 2));
            User = UserDataBase[login];
            return true;
        }

        public static bool UpdateCustomer(UserModel userModel)
        {
            return true;
        }
#if (APISIM)
        public static Seller GetSellerById(int id)
        {
            if (!SellerDataBase.ContainsKey(id))
            {
                throw new Exception("Wrong id Seller");
            }
            return SellerDataBase[id];
        }
#else
        public static async Task<Seller> GetSellerById(int id)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync($"{string.Format("http://134.86.19.105:5001/api/Sellers/",id , 8)}").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var seller = JsonConvert.DeserializeObject<Seller>(res);

            return seller;
        }
#endif
        private static List<string> items = new List<string>()
                    {
                            "A1","A2","A3","A4","A5",
                            "A6","A7","A8","A9","A10",
                            "A11","A12","A13","A14","B1",
                            "B2","B3","B4","B5","B6"
                    };
    }
}