using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MobileApplication.Src.API
{
    public static class CatalogAPIConection
    {
        private static Dictionary<int, List<int>> CatalogDataBase;

        static CatalogAPIConection()
        {
            CatalogDataBase = new Dictionary<int, List<int>>();

        }

        public static int GetSellerIdByProductId(int id)
        {
            return 1;
        }

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

        public static IEnumerable<int> GetProductsBySellerId(int sellerId)
        {
            return CatalogDataBase[sellerId];
        }


        public static async Task<int> GetCatalogIdByProductId(int id)
        {
            var client = new HttpClient();
            string ulr = string.Format("http://134.86.19.105:5003/api/sellerproduct/", id);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var catalogId = JsonConvert.DeserializeObject<int>(res);
            return catalogId;
        }
    }
}