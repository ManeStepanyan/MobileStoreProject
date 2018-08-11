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
        private IHttpContextAccessor _httpContextAccessor;
        public ShopController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> SellerAsync()
        {
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/sellers")  ;
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
        public async Task<IActionResult> Index(string msg = null, bool flag = true)
         {
            if(msg != null)
            {
                if(flag == true)
                    ViewBag["success_msg"] = msg;
                else ViewBag["warning_msg"] = msg;
            }

            // ... Target page.
            Uri idSiteUri = new Uri("http://localhost:5003/api/SellerProduct/products/" + _httpContextAccessor.HttpContext.Request.Cookies["seller_id"]);
             

             // ... Use HttpClient.
             using (HttpClient client = new HttpClient())
             {
                 client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                 using (HttpResponseMessage response = await client.GetAsync(idSiteUri))
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
        public async Task<IActionResult> Post(ProductModel instance)
        {
            Uri siteUri = new Uri("http://localhost:5003/api/SellerProduct");

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                var content = JsonConvert.SerializeObject(instance);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Shop", new { succes_msg = "The product was successfuly added" , flag = true});
                    }
                    else
                    {
                        return RedirectToAction("Index", "Shop", new { warning_msg = "The product was successfuly added", flag = false });
                    }
                }
            }   
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
                using (HttpResponseMessage response = await client.PutAsync(
                siteUri, byteContent))
                {
             //       var responseString = await response.Content.ReadAsStringAsync();
              //      var responseStr = JsonConvert.DeserializeObject<String>(responseString);
                    return RedirectToAction("Index", "Shop");
                }
            }
        }
    }   
}
