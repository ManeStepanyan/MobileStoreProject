using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersAndShopCartAPI.Models;

namespace OrdersAndShopCartAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ShopCart")]
    public class ShopCartController : Controller
    {
        private readonly Repo<ShopCart> repo;
        public ShopCartController(Repo<ShopCart> repo)
        {
            this.repo = repo;
        }
        // GET: api/ShopCart
        [HttpGet]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> Get()
        { int currentCustomerId;
            List<Product> list = new List<Product>();
            var userId = int.Parse(
                       ((ClaimsIdentity)this.User.Identity).Claims
                       .Where(claim => claim.Type == "user_id").First().Value);
            using (var customerClient = new HttpClient())
            {
                customerClient.BaseAddress = new Uri("http://localhost:5001/");
                customerClient.DefaultRequestHeaders.Accept.Clear();
                customerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var catalogIds = (IEnumerable<int>)(await this.repo.ExecuteOperationAsync("GetCatalogsByCustomerId", new[] { new KeyValuePair<string, object>("id", currentCustomerId) }));

            using (var catalogClient = new HttpClient())
            {
                catalogClient.BaseAddress = new Uri("http://localhost:5003/");
                catalogClient.DefaultRequestHeaders.Accept.Clear();
                catalogClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (var id in catalogIds)
                {
                    HttpResponseMessage response = await catalogClient.GetAsync("/api/sellerproduct/catalog/" +id);
                    if (response.IsSuccessStatusCode)
                    {
                        list.Add((Product)(await response.Content.ReadAsAsync(typeof(Product))));
                    }
                    return NotFound();
                }
            }
            return Ok(list);
        }

        // GET: api/ShopCart/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/ShopCart
        [HttpPost]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> Post([FromBody]int catalogId)
        { int currentCustomerId;
            var userId = int.Parse(
                       ((ClaimsIdentity)this.User.Identity).Claims
                       .Where(claim => claim.Type == "user_id").First().Value);
            using (var customerClient = new HttpClient())
            {
                customerClient.BaseAddress = new Uri("http://localhost:5001/");
                customerClient.DefaultRequestHeaders.Accept.Clear();
                customerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
      var res= await this.repo.ExecuteOperationAsync("AddToShopCart", new[] { new KeyValuePair<string, object>("CustomerId", currentCustomerId), new KeyValuePair<string, object>("CatalogId", catalogId) });
            if (res == null) return NotFound();
            return new StatusCodeResult(200);
        }
        
        // PUT: api/ShopCart/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int catalogId)
        {
            int currentCustomerId;
            var userId = int.Parse(
                       ((ClaimsIdentity)this.User.Identity).Claims
                       .Where(claim => claim.Type == "user_id").First().Value);
            using (var customerClient = new HttpClient())
            {
                customerClient.BaseAddress = new Uri("http://localhost:5001/");
                customerClient.DefaultRequestHeaders.Accept.Clear();
                customerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                currentCustomerId = customer.Id;
            }
            var res = await this.repo.ExecuteOperationAsync("DeleteFromShopCart", new[] { new KeyValuePair<string, object>("CustomerId", currentCustomerId), new KeyValuePair<string, object>("CatalogId", catalogId) });
            if (res == null) return NotFound();
            return new StatusCodeResult(200);
        }
    }
}
