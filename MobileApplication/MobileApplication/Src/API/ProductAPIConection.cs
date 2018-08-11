#define APISIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using MobileApplication.Src.Models;
using Newtonsoft.Json;

namespace MobileApplication.Src.API
{
    public static class ProductAPIConection
    {
        private static Dictionary<string, Product> ProductDataBase;
        private static int IndexId = 0;
#if (!APISIM)
        private static string ulr = string.Format("http://134.86.19.105:5002/api/Products", 8);
#endif

        static ProductAPIConection()
        {
            IndexId = 0;
            ProductDataBase = new Dictionary<string, Product>();
            //UserAPIConection.LogOut();
        }

        public static List<string> GetBrands()
        {
            return new List<string>()
            {
                "Nokia",
                "Samsung",
                "HTC",
                "HUAWEI",
                "LG",
                "Xiaomi",
                "ArmPhone",
                "Symens"
            };
        }

        public static void AddProduct(int sellerId, Product product)
        {
            product.Id = IndexId++;
            CatalogAPIConection.AddProduct(sellerId, product.Id);
            ProductDataBase.Add(product.Name, product);
        }

#if (APISIM)
        public static async Task<Product> GetProductById(int id)
        {
            foreach (var item in ProductDataBase)
            {
                if (id == item.Value.Id)
                {
                    return item.Value;
                }
            }

            return null;
        }
#else
        public static async Task<Product> GetProductById(int id)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr + "/" + id.ToString()).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<Product>(res);

            return product;
        }
#endif



#if (APISIM)
        public static List<Product> SearchProduct(string name)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => a.Name.IndexOf(name) != -1).ToList();
        }
#else
        public static Task<List<Product>> SearchProduct(string name)
        {

        }
#endif


#if (APISIM)
        public static async Task<List<Product>> SearchProduct(SearchProductModel searchProductModel)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => ChechSerach(searchProductModel, a)).ToList();
        }
#else
        public static async Task<List<Product>> SearchProduct(SearchProductModel searchProductModel)
        {
            Uri siteUri = new Uri("http://192.168.6.30:5002/api/Products/Search/");

            using (HttpClient client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(searchProductModel);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(siteUri, byteContent).Result;
                var res = response.Content.ReadAsStringAsync().Result;
                var products = JsonConvert.DeserializeObject<List<Product>>(res);

                return products;
            }
        }
#endif

        private static bool ChechSerach(SearchProductModel model, Product product)
        {
            return ((model.Price == null) || ((double)model.Price < (double)product.Price)) &&
                    ((model.PriceTo == null) || ((double)model.PriceTo > (double)product.Price)) &&
                    ((model.RAM == null) || (model.RAM < product.RAM)) &&
                    ((model.RAMTo == null) || (model.RAMTo > product.RAM)) &&
                    ((model.Year == null) || (model.Year < product.Year)) &&
                    ((model.YearTo == null) || (model.YearTo > product.Year)) &&
                    ((model.Battery == null) || (model.Battery < product.Battery)) &&
                    ((model.BatteryTo == null) || (model.BatteryTo > product.Battery)) &&
                    ((model.Camera == null) || (model.Camera > product.Camera)) &&
                    ((model.CameraTo == null) || (model.CameraTo < product.Camera)) &&
                    ((model.Memory == null) || (model.Memory > product.Memory)) &&
                    ((model.MemoryTo == null) || (model.MemoryTo < product.Memory)) &&
                    ((model.Brand == "") || (product.Brand.IndexOf(model.Brand) != -1));
        }

#if (!APISIM)
        public static Task<List<Product>> GetProducts() => new Task<List<Product>>(() =>  ProductDataBase.Select(a => a.Value).ToList());
#else
        public static async Task<List<Product>> GetProducts()
        {
            var client = new HttpClient();
            string ulr = string.Format("http://134.86.19.105:5002/api/Products");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<List<Product>>(res);

            return products;
        }

#endif

    }
}