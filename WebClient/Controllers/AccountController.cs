using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebClient.Models;
using WebClient.Services;
using System.Web;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{

    public class AccountController : Controller
    {
        AccountService _accountService;
        public IActionResult LoginView()
        {
            return View();
        }

        // GET: /<controller>/
        [Route("account/login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return new JsonResult(404);
            }
            var tokenClient = new TokenClient(disco.TokenEndpoint, "SuperAdmin", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(login, password, "openid");

            
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return new JsonResult(404);
            }

            var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint);
            var identityClaims = await userInfoClient.GetAsync(tokenResponse.AccessToken);
            var claims = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(identityClaims.Json.ToString());

            HttpContext.Session.SetInt32("user_id", (int)claims["user_id"][0]);
            HttpContext.Session.SetInt32("role", (int)claims["role"][0]);
            HttpContext.Session.SetInt32("Is logged", 1);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1d);
            Response.Cookies.Append("role", (string)claims["role"][0], option);
            Response.Cookies.Append("role", (string)claims["user_id"][0], option);
            return RedirectToAction("Index","Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        // GET: /<controller>/
        public IActionResult RegisterCustomerView(string ErrorMsg = null)
        {
            ViewData["ErrorMessage"] = ErrorMsg;
            return View();
        }


        public IActionResult RegisterSellerView(string ErrorMsg = null)
        {

            ViewData["ErrorMessage"] = ErrorMsg;
            return View();
        }

        public async Task<IActionResult> RegisterSellerAsync(string name, string address, string cellphone, string login, string email, string password)
        {
            var model = new UserModel(name, null, cellphone, address, login, password, email, 2);
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/Register");

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.SerializeObject(responseString);
                    var responseStr = JsonConvert.DeserializeObject<string>(json);
                    if (responseStr == "Registration has been done,And Account activation link has been sent your email:" + email)
                        return RedirectToAction("Index", "Home", new { msg = responseStr.ToString() });
                    else
                        return RedirectToAction("RegisterSellerView", "Account", new { ErrorMsg = responseStr.ToString() });
                }
            }
        }

        public async Task<IActionResult> RegisterCustomerAsync(string name, string surename, string login, string email, string password)
        {
            var model = new UserModel(name, surename, null, null, login, password, email, 3);

            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/Register");
            CustomerModel customer = new CustomerModel();
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseStr = JsonConvert.DeserializeObject<String>(responseString);
                    if (responseStr == "Registration has been done,And Account activation link has been sent your email:" + email)
                        return RedirectToAction("Index", "Home", new { msg = responseStr });
                    else
                        return RedirectToAction("RegisterCustomerView", "Account", new { ErrorMsg = responseStr });
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }
    }
}
