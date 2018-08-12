//#define APISIM
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.Graphics;
using IdentityModel.Client;
using MobileApplication.Src.IP;
using MobileApplication.Src.Models;
using Newtonsoft.Json;

namespace MobileApplication.Src.API
{
    public static class UserAPIController
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
        public static string SessionToken { get; private set; }
        

#if (APISIM)
        static UserAPIController()
        {
            var ran = new Random();
            var images = new List<string>() {
                @"https://images.samsung.com/is/image/samsung/p5/ru/smartphones/PCD_Purple.png?$ORIGIN_PNG$",
                @"https://drop.ndtv.com/TECH/product_database/images/6142017123101PM_635_htc_desire_628_dual_sIM.jpeg",
                @"https://akket.com/wp-content/uploads/2018/04/Nokia-9-42.jpg",
                @"https://3dnews.ru/assets/external/illustrations/2018/03/16/967085/sm.x1.750.jpg"
            };
            var Products = new List<Product>();
            var brannds = ProductAPIController.GetBrands();
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
                ProductAPIController.AddProduct(seller.Id, Products[index++]);
                ProductAPIController.AddProduct(seller.Id, Products[index++]);
                ProductAPIController.AddProduct(seller.Id, Products[index++]);
            }
        }
#endif


#if (APISIM)
        public static bool LogOut()
        {
            if (User == null)
            {
                return false;
            }
            User = null;

            return true;
        }
#else
        public static bool LogOut()
        {
            if (User == null)
            {
                return false;
            }
            User = null;
            SessionToken = null;
            return true;
        }
#endif
        public static bool SessionActivity() => (User != null) ? true : false;

#if (APISIM)
        public static bool SigeIn(string Login, string password)
        {
            if (!UserDataBase.ContainsKey(Login) || UserDataBase[Login].Password != password)
            {
                return false;
            }

            User = UserDataBase[Login];
            return true;
        }
#else
        /// <summary>
        /// Customer SigeIn.
        /// </summary>
        /// <param name="login">Customer login.</param>
        /// <param name="password">Customer password.</param>
        /// <returns>Is success. (bool)</returns>
        public static bool SigeIn(string login, string password)
        {
            
            var client = new DiscoveryClient($"http://{Resource.String.ip}:5000");
            client.Policy.RequireHttps = false;

            var disco = client.GetAsync().Result;

            
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return false;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "SuperAdmin", "secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(login, password).Result; //


            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return false;
            }

            var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint);
            var identityClaims = userInfoClient.GetAsync(tokenResponse.AccessToken).Result;
            SessionToken = identityClaims.Json.ToString();
            var claims = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(SessionToken);
            var res = claims["user_id"] as Newtonsoft.Json.Linq.JArray;
            var UserId = int.Parse(res[0].ToString());
            //User = GetSellerById(UserId);
            return true;
        }


#endif
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
        /// <summary>
        /// Get seller by seller Id.
        /// </summary>
        /// <param name="id">seller Id</param>
        /// <returns>Seller</returns>
        public static async Task<Seller> GetSellerById(int id)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync($"{string.Format($"http://{IPConfig.IP}:5001/api/Sellers/", id, 8)}").Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var seller = JsonConvert.DeserializeObject<Seller>(res);

            return seller;
        }
#endif
#if (APISIM)
        private static List<string> items = new List<string>()
                    {
                            "A1","A2","A3","A4","A5",
                            "A6","A7","A8","A9","A10",
                            "A11","A12","A13","A14","B1",
                            "B2","B3","B4","B5","B6"
                    };
#endif
    }
}