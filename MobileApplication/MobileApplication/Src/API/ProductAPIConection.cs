#define APISIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MobileApplication.Src.Models;
using Newtonsoft.Json;

namespace MobileApplication.Src.API
{
    public static class ProductAPIConection
    {
        private static Dictionary<string, Product> ProductDataBase;
        private static int IndexId = 0;

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

        public static Product GetProductById(int id)
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

        public static List<Product> SearchProduct(string name)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => a.Name.IndexOf(name) != -1).ToList();
        }

        public static List<Product> SearchProduct(SearchProductModel searchProductModel)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => ChechSerach(searchProductModel, a)).ToList();
        }

        private static bool ChechSerach(SearchProductModel model, Product product)
        {
            return ((model.MinPrice == null) || (model.MinPrice < product.Price)) &&
                    ((model.MaxPrice == null) || (model.MaxPrice > product.Price)) &&
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
                    ((model.Brand == "") || (product.Brand.IndexOf(model.Brand) != -1))
                    ;
        }
#if (APISIM)
        public static List<Product> GetProducts() => ProductDataBase.Select(a => a.Value).ToList();
#else
        public static async Task<List<Product>> GetProducts()
        {
            Uri siteUri = new Uri("http://localhost:5002/api/Products");

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(siteUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        var sellers = JsonConvert.DeserializeObject<List<Product>>(result);
                        if (result != null &&
                            result.Length >= 50)
                        {
                            Console.WriteLine(result.Substring(0, 50) + "...");

                        }
                        return sellers;
                    }
                }
            }
        }

#endif
        public static List<Product> GetProductsById(int id)
        {
            return ProductDataBase.Select(a => a.Value).Where(a => a.Id == id).ToList();
        }
    }
}