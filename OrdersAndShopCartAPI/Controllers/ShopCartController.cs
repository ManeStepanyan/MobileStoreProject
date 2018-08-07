using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OrdersAndShopCartAPI.Models;

namespace OrdersAndShopCartAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ShopCart")]
    public class ShopCartController : Controller
    {
        private readonly Repo<ShopCart> repo;
        private IHttpContextAccessor _httpContextAccessor;
        public ShopCartController(Repo<ShopCart> repo, IHttpContextAccessor httpContextAccessor)
        {
            this.repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: api/ShopCart
        [HttpGet]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> Get()
        {
            int currentCustomerId;
            List<Product> list = new List<Product>();
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/users" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var catalogIds = (IEnumerable<int>)(await this.repo.ExecuteOperationAsync("GetCatalogsByCustomerId", new[] { new KeyValuePair<string, object>("id", currentCustomerId) }));
            if (catalogIds != null)
            {
                using (var catalogClient = InitializeClient("http://localhost:5003/"))
                {
                    foreach (var id in catalogIds)
                    {
                        HttpResponseMessage response = await catalogClient.GetAsync("/api/sellerproduct/catalog/" + id);
                        if (response.IsSuccessStatusCode)
                        {
                            list.Add((Product)(await response.Content.ReadAsAsync(typeof(Product))));
                        }
                        return NotFound();
                    }
                }
                return Ok(list);
            }
            return NotFound();
        }

        // GET: api/ShopCart/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ShopCart
        [HttpPost]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> Post([FromBody]JToken catalogId)
        {
            int currentCustomerId;
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
               
                var a1 = _httpContextAccessor.HttpContext.Request.Cookies;
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/users" + userId);
                if (!response.IsSuccessStatusCode) return NotFound();
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var res = await this.repo.ExecuteOperationAsync("AddToShopCart", new[] { new KeyValuePair<string, object>("CustomerId", currentCustomerId), new KeyValuePair<string, object>("CatalogId", catalogId) });
            if (res == null) return NotFound();
            return Ok();
        }

        // PUT: api/ShopCart/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody]JToken  catalogId)
        {
            int currentCustomerId;
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {              
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/users" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var res = await this.repo.ExecuteOperationAsync("DeleteFromShopCart", new[] { new KeyValuePair<string, object>("CustomerId", currentCustomerId), new KeyValuePair<string, object>("CatalogId", catalogId) });
            if (res == null) return NotFound();
            return Ok();
        }
        public HttpClient InitializeClient(string uri)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            var authInfo = _httpContextAccessor.HttpContext.AuthenticateAsync();
            var token = authInfo.Result.Properties.Items.Values.ElementAt(0);
            client.SetBearerToken(token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public int GetCurrentUser()
        {
            return int.Parse(((ClaimsIdentity)this.User.Identity).Claims.Where(claim => claim.Type == "user_id").First().Value);
        }
    }
}
