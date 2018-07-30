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
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
    public class ShopController : Controller
    {
        //List<ProductModel> currentProducts = new List<ProductModel>();
        // GET: /<controller>/
      /*  public IActionResult Index()
        {
            return View();
        } */

        public async Task<IActionResult> SellerAsync()
        {
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/sellers");
            List<SellerModel> sellers = new List<SellerModel>();

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(siteUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        sellers = JsonConvert.DeserializeObject<List<SellerModel>>(result);

                        if (result != null &&
                            result.Length >= 50)
                        {
                            Console.WriteLine(result.Substring(0, 50) + "...");
                        }
                    }
                }
            }
            return View(sellers);
        }

        //[Route("home/seller/{id}")]
        public async Task<IActionResult> Index()
         {
             // ... Target page.
             Uri idSiteUri = new Uri("http://localhost:5003/api/SellerProduct/products/1");
             

             // ... Use HttpClient.
             using (HttpClient client = new HttpClient())
             {
                 using (HttpResponseMessage response = await client.GetAsync(idSiteUri))
                    {
                     using (HttpContent content = response.Content)
                     {
                        // ... Read the string.
                        string result = await content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<ProductModel>>(result);
                        //this.currentProducts = products;
                        /*if(product_ids == null)
                            Response.StatusCode = 500;
                        string prodSiteUri = "http://localhost:5002/api/Products/";
                        foreach(var prod in product_ids)
                        {
                            prodSiteUri += $"&{prod.Key}={prod.Value}";
                        }
                        using (HttpResponseMessage res = await client.GetAsync(prodSiteUri))
                        {
                            using (HttpContent content1 = res.Content)
                            {
                                string res1 = await content1.ReadAsStringAsync();
                                var products = JsonConvert.DeserializeObject<List<ProductModel>>(res1);
                                if (result != null &&
                                result.Length >= 50)
                                {
                                    Console.WriteLine(result.Substring(0, 50) + "...");
                                }
                            }
                        }*/
                        return View(products);

                    }
                }
             }
         }
        public async Task<IActionResult> Post(ProductModel instance)
        {
            return View();
        }

        public async Task<IActionResult> UpdateViewAsync(int id)
        {
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5002/api/Products/"+id);
            var product = new ProductModel();
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

        public async Task<IActionResult> Update(ProductModel instance)
        {
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5002/api/Products/"+instance.Id);
            CustomerModel customer = new CustomerModel();
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(instance);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseStr = JsonConvert.DeserializeObject<String>(responseString);
                    return RedirectToAction("Index", "Shop", new { msg = responseStr });
                }
            }
        }
    }   
}
