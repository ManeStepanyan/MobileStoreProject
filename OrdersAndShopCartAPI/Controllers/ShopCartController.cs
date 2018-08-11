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
        /// <summary>
        ///  Repository to manage method calls to db
        /// </summary>
        private readonly Repo<ShopCart> repo;
        /// <summary>
        /// http context accessor
        /// </summary>
        private IHttpContextAccessor _httpContextAccessor;
        public ShopCartController(Repo<ShopCart> repo, IHttpContextAccessor httpContextAccessor)
        {
            this.repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: api/ShopCart
        /// <summary>
        /// Returning current user's shopcart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> Get()
        {
            List<KeyValuePair<int,int>> catalogIdsAndQuantites = new List<KeyValuePair<int,int>>();
            int currentCustomerId;
            List<KeyValuePair<Product,int>> list = new List<KeyValuePair<Product,int>>();
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                var response = await customerClient.GetAsync("/api/customers/users/" + userId);
                var customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var info = (IEnumerable<ShopCart>)(await this.repo.ExecuteOperationAsync("GetCatalogsByCustomerId", new[] { new KeyValuePair<string, object>("id", currentCustomerId) }));
            foreach(var item in info)
            {
                catalogIdsAndQuantites.Add(new KeyValuePair<int, int>(item.CatalogId,item.Quantity));

            }
            if (catalogIdsAndQuantites != null)
            {
                using (var catalogClient = InitializeClient("http://localhost:5003/"))
                {
                    foreach (var id in catalogIdsAndQuantites)
                    {
                        var response = await catalogClient.GetAsync("/api/sellerproduct/catalog/" + id.Key);
                        if (response.IsSuccessStatusCode)
                        {
                            list.Add(new KeyValuePair<Product, int>((Product)(await response.Content.ReadAsAsync(typeof(Product))),id.Value));
                        }
                       else return NotFound();
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
        /// <summary>
        /// Adding product to shopcart by current customer
        /// </summary>
        /// <param name="catalogId">catalog id </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> Post([FromBody]int catalogId)
        {
            int currentCustomerId;
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/users/" + userId);
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
        /// <summary>
        /// deleting product from shopcart
        /// </summary>
        /// <param name="catalogId">catalog id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody]JToken  catalogId)
        {
            int currentCustomerId;
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {              
                var response = await customerClient.GetAsync("/api/customers/users" + userId);
                var customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var res = await this.repo.ExecuteOperationAsync("DeleteFromShopCart", new[] { new KeyValuePair<string, object>("CustomerId", currentCustomerId), new KeyValuePair<string, object>("CatalogId", catalogId) });
            if (res == null) return NotFound();
            return Ok();
        }
        /// <summary>
        /// initialzing http client
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
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
