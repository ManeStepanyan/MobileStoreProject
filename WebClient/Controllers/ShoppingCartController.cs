using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: /<controller>/
        public async Task<bool> AddAsync(int id)
        {
            var Uri = new Uri("http://localhost:5003/api/SellerProduct/GetCatalogIdByProductId/" + id);


            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(Uri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        var catid = JsonConvert.DeserializeObject<KeyValuePair<string, int>>(result);
                        var UriToAdd = new Uri("http://localhost:5005/api/ShopCart/" + catid.Value);
                        using (HttpResponseMessage res = await client.GetAsync(UriToAdd))
                        {
                            using (HttpContent content1 = res.Content)
                            {
                                string res1 = await content1.ReadAsStringAsync();
                                var a = JsonConvert.DeserializeObject(res1);
                                if(a.Equals(200))
                                    return true;
                                else return false;
                            }
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> GetAsync()
        {
            Uri Uri = new Uri("http://localhost:5005/api/ShopCart");
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(Uri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<ProductModel>>(result);
                        return View(products);
                    }
                }
            }
        }
    }
}
