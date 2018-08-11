using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MobileApplication.Src.API
{
    public static class OrdersAndShopCartAPIConection
    {
        private static List<int> CartDataBase;

        static OrdersAndShopCartAPIConection()
        {
            CartDataBase = new List<int>();
            //for (var i = 0; i < 10; ++i)
            //{
            //    CartDataBase.Add(i);
            //}
        }
#if (APISIM)
        public static bool AddProduct(int id)
        {
            if (CartDataBase.Contains(id))
            {
                return false;
            }

            CartDataBase.Add(id);

            return true;
        }
#else
        /// <summary>
        /// Add Product in Shop Cart.
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        public static async Task<bool> AddProduct(int id)
        {
            var catalogId = CatalogAPIConection.GetCatalogIdByProductId(id);
            using (HttpClient client = new HttpClient())
            {
                //To Do
                //client.SetBearerToken(UserAPIConection.SessionToken);
                var cont = JsonConvert.SerializeObject(catalogId);
                var buffer = System.Text.Encoding.UTF8.GetBytes(cont);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var UriToAdd = new Uri("http://134.86.19.105:5005/api/ShopCart/");

                using (var res = client.PostAsync(UriToAdd, byteContent).Result)
                {
                    return res.IsSuccessStatusCode; 
                }
            }
        }
#endif

        public static bool DeleteProduct(int id)
        {
            if (!CartDataBase.Contains(id))
            {
                return false;
            }

            CartDataBase.Remove(id);

            return true;
        }
        
        public static IEnumerable<int> GetAllProducts()
        {
            return CartDataBase;
        }
    }
}