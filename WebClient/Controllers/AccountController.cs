﻿using IdentityModel.Client;
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
using Microsoft.AspNetCore.Authentication;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{

    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult LoginView()
        {
            return View();
        }

        // GET: /<controller>/
        [Route("account/login")]
        public async Task<IActionResult> Login(string login, string password) {

            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                return NotFound();
            }
            var tokenClient = new TokenClient(disco.TokenEndpoint, "SuperAdmin", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(login, password); //

            
            if (tokenResponse.IsError)
            {
                return NotFound();
            }

            var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint);
            var identityClaims = await userInfoClient.GetAsync(tokenResponse.AccessToken);
            var claims = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(identityClaims.Json.ToString());

            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1d)
                
            };
            if(claims["role"] == "2")
                Response.Cookies.Append("seller_id", (string)claims["user_id"], option);
            else if(claims["role"] == "3")
                Response.Cookies.Append("customer_id", (string)claims["user_id"], option);

            Response.Cookies.Append("role", (string)claims["role"], option);
            Response.Cookies.Append("Is logged", "1", option);
            Response.Cookies.Append("token", tokenResponse.AccessToken);
            return RedirectToAction("Index","Home");

        }

        public IActionResult SignUp()
        {
            return View();
        }
        // GET: /<controller>/
        public IActionResult RegisterCustomerView(string msg = null , bool flag = true)
        { 
            if(msg != null)
            {
                if (flag == true)
                    ViewData["SuccessMessage"] = msg;
                else
                    ViewData["ErrorMessage"] = msg;

            }
            return View();
        }


        public IActionResult RegisterSellerView(string msg = null, bool flag = true)
        {

            if (msg != null)
            {
                if (flag == true)
                    ViewData["SuccessMessage"] = msg;
                else
                    ViewData["ErrorMessage"] = msg;

            }
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
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("RegisterSellerView", "Account", new { msg = "Registration has been done,And Account activation link has been sent to your email:"+email+ " please open the link in message to verify your account and use it", flag = true });
                    if ((int)response.StatusCode == 412)
                        return RedirectToAction("RegisterSellerView", "Account", new { msg = "Such an username already exist's please change it", flag = false });
                    if ((int)response.StatusCode == 416)
                        return RedirectToAction("RegisterSellerView", "Account", new { msg = "Such an email exist's please try change it", flag = false });
                }
                return RedirectToAction("RegisterSellerView", "Account", new { msg = "Some data was incorrect please try again", flag = false });
            }
        }

        public async Task<IActionResult> RegisterCustomerAsync(string name, string surname, string login, string email, string password)
        {
            var model = new UserModel(name, surname, null, null, login, password, email, 3);
            // ... Target page.
            Uri siteUri = new Uri("http://localhost:5001/api/Register");
            CustomerModel customer = new CustomerModel();
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(model);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpResponseMessage response = await client.PostAsync(
                siteUri, byteContent))
                {
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("RegisterCustomerView", "Account", new { msg = "Registration has been done,And Account activation link has been sent to your email:" + email + " please open the link in message to verify your account and use it", flag = true });
                    if ((int)response.StatusCode == 412)
                        return RedirectToAction("RegisterCustomerView", "Account", new { msg = "Such an username already exists please change it", flag = false });
                    if ((int)response.StatusCode == 416)
                        return RedirectToAction("RegisterCustomerView", "Account", new { msg = "Such an email exists please try change it", flag = false });
                }
                return RedirectToAction("RegisterCustomerView", "Account", new { msg = "Some data was incorrect please try again", flag = false });
            }
        }

        public IActionResult Logout()
        {
            var allCookies = Request.Cookies;
            foreach (var cookie in allCookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }
    }
}
