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

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            var allCookies = Request.Cookies;
            var r = _httpContextAccessor.HttpContext.Request.Cookies["role"];
            var i = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            var t = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

            public async Task<IActionResult> ProductsAsync()
            {
                Uri siteUri = new Uri("http://localhost:5002/api/Products");
                List<ProductModel> sellers = new List<ProductModel>();

                // ... Use HttpClient.
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(siteUri))
                    {
                        using (HttpContent content = response.Content)
                        {
                            // ... Read the string.
                            string result = await content.ReadAsStringAsync();
                            sellers = JsonConvert.DeserializeObject<List<ProductModel>>(result);

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
        }
    }
