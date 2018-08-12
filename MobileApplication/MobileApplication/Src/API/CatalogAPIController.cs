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
    public static class CatalogAPIController
    {
        private static Dictionary<int, List<int>> CatalogDataBase;

        static CatalogAPIController()
        {
            CatalogDataBase = new Dictionary<int, List<int>>();

        }

#if (APISIM)
        public static int GetSellerIdByProductId(int id)
        {
            return 1;
        }
#else
        /// <summary>
        /// Get seller id by Product id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>int id.</returns>
        public static async Task<int> GetSellerIdByProductId(int productId)
        {
            var client = new HttpClient();
            var ulr = $"http://{IPConfig.IP}:5003/api/SellerProduct/seller/{productId}";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var sellerId = JsonConvert.DeserializeObject<int>(res);
            return sellerId;
        }
#endif


#if (APISIM)
        public static void AddProduct(int sellerId, int productId)
        {
            if (CatalogDataBase.ContainsKey(sellerId))
            {
                CatalogDataBase[sellerId].Add(productId);
            }
            else
            {
                CatalogDataBase.Add(sellerId, new List<int>() { productId });
            }
        }
#else
        public static void AddProduct(int sellerId, int productId)
        {
            throw new Exception();
        }
#endif
#if (APISIM)
        public static IEnumerable<Product> GetProductsBySellerId(int sellerId)
        {
            return CatalogDataBase[sellerId].Select(id => ProductAPIController.GetProductById(id).Result);
        }
#else
        /// <summary>
        /// Get products by seller id.
        /// </summary>
        /// <param name="sellerId">Seller Id.</param>
        /// <returns>IEnumerable<Product>.</returns>
        public static IEnumerable<Product> GetProductsBySellerId(int sellerId)
        {
            var client = new HttpClient();
            var ulr = $"http://{IPConfig.IP}:5003/api/SellerProduct/products/{sellerId}";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(res);
            return products;
        }
#endif

        /// <summary>
        /// get Catalog id by Product Id.
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>int id</returns>
        public static async Task<int> GetCatalogIdByProductId(int productId)
        {
            var client = new HttpClient();
            var ulr = $"http://{IPConfig.IP}:5003/api/sellerproduct/{productId}";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var catalogId = JsonConvert.DeserializeObject<int>(res);
            return catalogId;
        }
    }
}