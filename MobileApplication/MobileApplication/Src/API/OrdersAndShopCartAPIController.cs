//#define APISIM
using MobileApplication.Src.IP;
using MobileApplication.Src.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MobileApplication.Src.API
{
    public static class OrdersAndShopCartAPIController
    {
        private static List<int> CartDataBase;

        static OrdersAndShopCartAPIController()
        {
            CartDataBase = new List<int>();
            //for (var i = 0; i < 10; ++i)
            //{
            //    CartDataBase.Add(i);
            //}
        }
#if (APISIM)
        public static async Task<bool> AddProduct(int id)
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
            var catalogId = CatalogAPIController.GetCatalogIdByProductId(id);
            using (HttpClient client = new HttpClient())
            {
                //To Do
                //client.SetBearerToken(UserAPIConection.SessionToken);
                var cont = JsonConvert.SerializeObject(catalogId);
                var buffer = System.Text.Encoding.UTF8.GetBytes(cont);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var UriToAdd = new Uri($"http://{Resource.String.ip}:5005/api/ShopCart/");

                using (var res = client.PostAsync(UriToAdd, byteContent).Result)
                {
                    return res.IsSuccessStatusCode; 
                }
            }
        }
#endif


#if (APISIM)
        public static bool DeleteProduct(int id)
        {
            if (!CartDataBase.Contains(id))
            {
                return false;
            }

            CartDataBase.Remove(id);

            return true;
        }
#else
        /// <summary>
        /// Delte product in Cart.
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <returns>Is Success.(bool)</returns>
        public static bool DeleteProduct(int id)
        {

            var Uri = new Uri($"http://{IPConfig.IP}:5003/api/sellerproduct/{id}");
            // ... Use HttpClient.
            using (var client = new HttpClient())
            {
                //client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                using (var response = client.DeleteAsync(Uri).Result)
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
#endif


#if (APISIM)
        public static IEnumerable<Product> GetAllProducts()
        {
            return CartDataBase.Select(id => ProductAPIController.GetProductById(id).Result);
        }
#else
        /// <summary>
        /// Get all Products in shop cart
        /// </summary>
        /// <returns>IEnumerable<Product></returns>
        public static IEnumerable<Product> GetAllProducts()
        {
            var Uri = new Uri($"http://{IPConfig.IP}:5005/api/ShopCart");
            // ... Use HttpClient.
            using (var client = new HttpClient())
            {
                //client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                using (var response = client.GetAsync(Uri).Result)
                {
                    using (var content = response.Content)
                    {
                        // ... Read the string.
                        string result = content.ReadAsStringAsync().Result;
                        var products = JsonConvert.DeserializeObject<List<KeyValuePair<Product, int>>>(result);
                        return products.Select(a => a.Key);
                    }
                }
            }
        }
#endif
    }
}