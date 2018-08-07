using System;
using System.Collections.Generic;
using System.Net;
using Android.Graphics;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.API
{
    public static class UserAPIConection
    {
        /// <summary>
        /// User anem User pair
        /// </summary>
        private static Dictionary<string, UserModel> UserDataBase;
        /// <summary>
        /// Seller id seller pair
        /// </summary>
        private static Dictionary<int, Seller> SellerDataBase;
        public static UserModel User { get; private set; }

        static UserAPIConection()
        {
            var ran = new Random();
            var bitMap = GetImageBitmapFromUrl(@"https://images.samsung.com/is/image/samsung/p5/ru/smartphones/PCD_Purple.png?$ORIGIN_PNG$");
            var Products = new List<Product>();
            foreach (var item in items)
            {
                var product = new Product()
                {
                    Name = item,
                    Price = ran.Next(1000, 10000),
                    Image = bitMap,
                    Quantity = 5,
                    Brand = "Nokia"
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

        public static bool RegisterCustomer(string name, string surename, string login, string email, string password)
        {
            if (UserDataBase.ContainsKey(login))
            {
                return false;
            }

            UserDataBase.Add(login, new UserModel(name, surename, null, null, login, password, email, 2));
            User = UserDataBase[login];
            return true;
        }


        public static Seller GetSellerById(int id)
        {
            if (!SellerDataBase.ContainsKey(id))
            {
                throw new Exception("Wrong id Seller");
            }
            return SellerDataBase[id];
        }

        private static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private static List<string> items = new List<string>()
                    {
                            "Afghanistan","Albania","Algeria","American Samoa","Andorra",
                            "Angola","Anguilla","Antarctica","Antigua and Barbuda","Argentina",
                            "Armenia","Aruba","Australia","Austria","Azerbaijan",
                            "Bahrain","Bangladesh","Barbados","Belarus","Belgium",
                    };
    }
}