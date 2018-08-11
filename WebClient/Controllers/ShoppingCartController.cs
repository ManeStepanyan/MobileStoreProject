using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ShoppingCartController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: /<controller>/
        //  [Authorize(Policy = "Customer")]
        public async Task<bool> AddAsync(int id)
        {
            var Uri = new Uri("http://localhost:5003/api/sellerproduct/" + id);

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                var authInfo = _httpContextAccessor.HttpContext.AuthenticateAsync();
                var token = authInfo.Result.Properties.Items.Values.ElementAt(0);
                var t = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                client.SetBearerToken(t);
                using (HttpResponseMessage response = await client.GetAsync(Uri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        var result = await content.ReadAsStringAsync();
                        var cont = JsonConvert.SerializeObject(result);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(result);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var UriToAdd = new Uri("http://localhost:5005/api/ShopCart/");

                        using (
                            HttpResponseMessage res = await client.PostAsync(UriToAdd, byteContent))
                        {
                            if (res.IsSuccessStatusCode) return true;
                            return false;
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> GetAsync()
        {
            var Uri = new Uri("http://localhost:5005/api/ShopCart");
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(_httpContextAccessor.HttpContext.Request.Cookies["token"]);
                using (HttpResponseMessage response = await client.GetAsync(Uri))
                {
                    using (HttpContent content = response.Content)
                    {
                        // ... Read the string.
                        var result = await content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<KeyValuePair<ProductModel,int>>>(result);
                        return View(products);
                    }
                }
            }
        }
    }
}
