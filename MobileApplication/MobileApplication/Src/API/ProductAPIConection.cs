#define APISIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        public static List<Product> SearchProduct(string name)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => a.Name.IndexOf(name) != -1).ToList();
        }

#if (APISIM)
        public static List<Product> SearchProduct(SearchProductModel searchProductModel)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => ChechSerach(searchProductModel, a)).ToList();
        }
#else
        public static async Task<List<Product>> SearchProduct(SearchProductModel searchProductModel)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(ulr + "/" + id.ToString()).Result;
            var res = response.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<List<Product>>(res);

            return products;
        }
    }
#endif

        private static bool ChechSerach(SearchProductModel model, Product product)
        {
            return ((model.MinPrice == null) || ((double)model.MinPrice < (double)product.Price)) &&
                    ((model.MaxPrice == null) || ((double)model.MaxPrice > (double)product.Price)) &&
                    ((model.MinRAM == null) || (model.MinRAM < product.RAM)) &&
                    ((model.MaxRAM == null) || (model.MaxRAM > product.RAM)) &&
                    ((model.MinYear == null) || (model.MinYear < product.Price)) &&
                    ((model.MaxYear == null) || (model.MaxYear > product.Price)) &&
                    ((model.MinBattery == null) || (model.MinBattery < product.Price)) &&
                    ((model.MaxBattery == null) || (model.MaxBattery > product.Price)) &&
                    ((model.MinCamera == null) || (model.MinCamera > product.Price)) &&
                    ((model.MaxCamera == null) || (model.MaxCamera < product.Price)) &&
                    ((model.MinMemory == null) || (model.MinMemory > product.Price)) &&
                    ((model.MaxMemory == null) || (model.MaxMemory < product.Price)) &&
                    ((model.Brand == "") || (product.Brand.IndexOf(model.Brand) != -1));
        }

#if (APISIM)
        public static Task<List<Product>> GetProducts() => new Task<List<Product>>(() =>  ProductDataBase.Select(a => a.Value).ToList());
#else
        public static async Task<List<Product>> GetProducts()
        {
            Uri siteUri = new Uri("http://134.86.19.105:5002/api/Products");
            var instance = new Product()
            {
                Price = 5000
            };

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                //client.SetBearerToken(IHttpContextAccessor.HttpContext.Request.Cookies["token"]);
                var content = JsonConvert.SerializeObject(instance);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(siteUri, byteContent).Result;
                //using (HttpResponseMessage response = await client.PostAsync(siteUri, byteContent))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        //return RedirectToAction("Index", "Shop", new { succes_msg = "The product was successfuly added", flag = true });
                //    }
                //    else
                //    {
                //        //return RedirectToAction("Index", "Shop", new { warning_msg = "The product was successfuly added", flag = false });
                //    }
                //}
            }




            //    var client = new HttpClient();

            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = client.GetAsync(ulr).Result;
            //    var res = response.Content.ReadAsStringAsync().Result;
            //    var products = JsonConvert.DeserializeObject<List<Product>>(res);

            //    return products;
            return null;
        }

#endif
        public static List<Product> GetProductsById(int id)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => a.Id == id).ToList();
        }
    }
}