using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CatalogAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/sellerproduct")]
    [Authorize(Policy ="Seller")]
    public class SellerProductController : ControllerBase
    {

        private readonly Repo<SellerProduct> repo;

        public SellerProductController(Repo<SellerProduct> repo)
        {
            this.repo = repo;
        }


        // GET: api/SellerProduct
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllSellerProducts");
            if (result == null)
                return new StatusCodeResult(204);

            return new JsonResult(result);
        }

        // GET: api/SellerProduct/5
        [HttpGet("{id}", Name = "GetProductBySellerId")]
        public async Task<IActionResult> GetProductBySellerId(int id)
        {
            var res = await this.repo.ExecuteOperationAsync("GetProductBySellerId", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return new StatusCodeResult(404);
            }
            return new JsonResult(res);
        }

        // GET: api/SellerProduct/5
        [HttpGet("{id}", Name = "GetSellerId")]
        public async Task<IActionResult> GetSellerId(int id)
        {
            var res = await this.repo.ExecuteOperationAsync("GetSellerId", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return new StatusCodeResult(404);
            }
            return new JsonResult(res);
        }


        // POST: api/SellerProduct
        [HttpPost]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Post([FromBody]JToken jsonbody)
        {
            using (var productClient = new HttpClient())
            {
                productClient.BaseAddress = new Uri("http://localhost:5002/");
                productClient.DefaultRequestHeaders.Accept.Clear();
                productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await productClient.PostAsync("/api/products/", content);
                int productId = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());
            }
            var userId = int.Parse(
                         ((ClaimsIdentity)this.User.Identity).Claims
                         .Where(claim => claim.Type == "user_id").First().Value);
            using (var sellerClient = new HttpClient())
            {
                sellerClient.BaseAddress = new Uri("http://localhost:5001/");
                sellerClient.DefaultRequestHeaders.Accept.Clear();
                sellerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/"+userId);
               SellerPublicInfo seller=(SellerPublicInfo)((await response.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                int sellerId = seller.Id;
            }
            return new StatusCodeResult(200);
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.ExecuteOperationAsync("DeleteSellerProduct", new[]
            {
                new KeyValuePair<string, object>("id", id)
            });
            return new StatusCodeResult(200);
        }
    }
}

