using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
    public class ProductController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ProductController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: /<controller>/
        public async Task<IActionResult> IndexAsync()
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
                        var sellers = JsonConvert.DeserializeObject<List<ProductModel>>(result);
                        if (result != null &&
                            result.Length >= 50)
                        {
                            Console.WriteLine(result.Substring(0, 50) + "...");

                        }
                        return View(sellers);
                    }
                }
            }
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            Uri siteUri = new Uri("http://localhost:5002/api/Products/"+id);
            ProductModel product = new ProductModel();

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(siteUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<ProductModel>(result);
                        if (result != null &&
                            result.Length >= 50)
                        {
                            Console.WriteLine(result.Substring(0, 50) + "...");
                        }
                    }
                }
            }
            return View(product);
        }

        public async Task<IActionResult> Search(SearchModel instance)//,decimal? priceTo = null,int? RAMTo = null,int? yearTo = null,int? batteryTo = null,int? cameraTo = null,int? memoryTo = null)
        {
            Uri siteUri = new Uri("http://localhost:5002/api/Products/Search/");
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                var content = JsonConvert.SerializeObject(instance);
                /*content += JsonConvert.SerializeObject(new { priceTo });
                content += JsonConvert.SerializeObject(new { RAMTo });
                content += JsonConvert.SerializeObject(new { yearTo });
                content += JsonConvert.SerializeObject(new { batteryTo });
                content += JsonConvert.SerializeObject(new { cameraTo });
                content += JsonConvert.SerializeObject(new { memoryTo });*/
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    using (HttpContent cont = response.Content)
                    {
                        // ... Read the string.
                        string result = await cont.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<ProductModel>>(result);
                        return View("~/Views/Product/IndexAsync", products);
                    }
                }
            }
        }
    }
}
