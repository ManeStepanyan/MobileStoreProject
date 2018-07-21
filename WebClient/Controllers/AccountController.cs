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
using IdentityModel.Client;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
   
    public class AccountController : Controller
    {
        public IActionResult LoginView()
        {
            return View();
        }

        // GET: /<controller>/
        [Route("account/login")]
        public async Task<IActionResult> Login(string login,string password)
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return new JsonResult(404);
            }
            var tokenClient = new TokenClient(disco.TokenEndpoint, "SuperAdmin", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(login,password,"UserAPI");

            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return new JsonResult(404);
            }
            return new JsonResult(tokenResponse.Json);
               
        }

        public IActionResult SignUp()
        {
            return View();
        }
        // GET: /<controller>/
        public  IActionResult RegisterCustomerView()
        {

            return View();
        }

        public async Task<bool> RegisterSellerAsync(string name, string address,string cellphone, string login, string email, string password)
        {
            var model = new SellerCreateModel(name, address, cellphone, login, email, password);
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/Sellers");
            SellerModel seller = new SellerModel();

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsJsonAsync(
                siteUri, model))
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        seller = JsonConvert.DeserializeObject<SellerModel>(result);
                        Console.WriteLine(seller);
                    }
                }
            }
            return true;
        }




        public IActionResult RegisterSellerView()
        {
            return View();
        }

        public async Task<bool> RegisterCustomerAsync(string name, string surename, string login, string email, string password)
        {
            var model = new CustomerModel(name, surename, login, email, password);
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/Customers");
            CustomerModel customer = new CustomerModel();

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsJsonAsync(
                siteUri, model))
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        customer = JsonConvert.DeserializeObject<CustomerModel>(result);
                    }
                }
            }
            return true;
        }

    }
}
