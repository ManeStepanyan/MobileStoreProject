using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CatalogAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/sellerproduct")]
    public class SellerProductController : Controller
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
        [HttpGet("products/{id}", Name = "GetProductsBySellerId")]
        public async Task<IActionResult> GetProductsBySellerId(int id)
        {
            List<Product> list = new List<Product>();
            var sellerProducts = (IEnumerable<SellerProduct>)(await this.repo.ExecuteOperationAsync("GetProductsBySellerId", new[] { new KeyValuePair<string, object>("id", id) }));

            using (var productClient = new HttpClient())
            {
                productClient.BaseAddress = new Uri("http://localhost:5002/");
                productClient.DefaultRequestHeaders.Accept.Clear();
                productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (var item in sellerProducts)
                {
                    HttpResponseMessage response = await productClient.GetAsync("/api/products/" + item.ProductId);
                    if (response.IsSuccessStatusCode)
                    {
                        list.Add((Product)(await response.Content.ReadAsAsync(typeof(Product))));
                    }
               else   return new StatusCodeResult(404);
                }
            }

            return new JsonResult(list);
        }
        [HttpGet("catalog/{id}", Name = "GetProductByCatalogId")]
        public async Task<IActionResult> GetProductsByCatalogId(int id)
        {
            Product product = new Product();
            int productId = (int)(await this.repo.ExecuteOperationAsync("GetProductByCatalogId", new[] { new KeyValuePair<string, object>("catalogId", id) }));

            using (var productClient = new HttpClient())
            {
                productClient.BaseAddress = new Uri("http://localhost:5002/");
                productClient.DefaultRequestHeaders.Accept.Clear();
                productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                    HttpResponseMessage response = await productClient.GetAsync("/api/products/" + productId);
                    if (response.IsSuccessStatusCode)
                    {
                      product=(Product)(await response.Content.ReadAsAsync(typeof(Product)));
                    }
                 else  return new StatusCodeResult(404);              
            }
            return new JsonResult(product);
        }

        // GET: api/SellerProduct/5
        // returns seller by specified product id
        [HttpGet("seller/{id}", Name = "GetSellerByProductId")]
        public async Task<IActionResult> GetSellerId(int id)
        {
            Object res;
            var sellerId = (int)await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            using (var sellerClient = new HttpClient())
            {
                sellerClient.BaseAddress = new Uri("http://localhost:5001/");
                sellerClient.DefaultRequestHeaders.Accept.Clear();
                sellerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/" + sellerId);
                res = await response.Content.ReadAsStringAsync();
            }
            if (res == null)
            {
                return new StatusCodeResult(404);
            }
            return new JsonResult(res);
        }

        [HttpGet("{id}", Name = "GetCatalogIdByProductId")]
        public async Task<IActionResult> GetCatalogIdByProductId(int id)
        {
            int catalogId = (int)await this.repo.ExecuteOperationAsync("GetByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            if (catalogId != 0)
            {
                return new JsonResult(catalogId);
            }
            return new JsonResult(404);
        }


        // POST: api/SellerProduct
        [HttpPost]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Post([FromBody]JToken jsonbody)
        {
            int productId, sellerId, catalogId;
            var userId = int.Parse(
                        ((ClaimsIdentity)this.User.Identity).Claims
                        .Where(claim => claim.Type == "user_id").First().Value);
            using (var productClient = new HttpClient())
            {
                productClient.BaseAddress = new Uri("http://localhost:5002/");
                productClient.DefaultRequestHeaders.Accept.Clear();
                productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await productClient.PostAsync("/api/products/", content);
                productId = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());


                using (var sellerClient = new HttpClient())
                {
                    sellerClient.BaseAddress = new Uri("http://localhost:5001/");
                    sellerClient.DefaultRequestHeaders.Accept.Clear();
                    sellerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resp = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                    SellerPublicInfo seller = (SellerPublicInfo)((await resp.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                    sellerId = seller.Id;
                }
                
                catalogId =Convert.ToInt32( await this.repo.ExecuteOperationAsync("AddSellerProduct", new[] { new KeyValuePair<string, object>("productId", productId), new KeyValuePair<string, object>("sellerId", sellerId) }));
                 content = new FormUrlEncodedContent(new[]
            {
             new KeyValuePair<string, string>("catalogId", catalogId.ToString())
        });
                response = await productClient.PutAsync("/api/products/catalog/" + productId, content);
            }
            return new StatusCodeResult(200);
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            int currentSellerId;
            var sellerId = (int)await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            var userId = int.Parse(
                         ((ClaimsIdentity)this.User.Identity).Claims
                         .Where(claim => claim.Type == "user_id").First().Value);
            using (var sellerClient = new HttpClient())
            {
                sellerClient.BaseAddress = new Uri("http://localhost:5001/");
                sellerClient.DefaultRequestHeaders.Accept.Clear();
                sellerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/" + userId);
                SellerPublicInfo seller = (SellerPublicInfo)((await response.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                currentSellerId = seller.Id;
            }
            if (currentSellerId == sellerId)
            {

                await this.repo.ExecuteOperationAsync("DeleteSellerProduct", new[]
                {
                new KeyValuePair<string, object>("id", id)
            }); using (var productClient = new HttpClient())
                {
                    productClient.BaseAddress = new Uri("http://localhost:5002/");
                    productClient.DefaultRequestHeaders.Accept.Clear();
                    productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await productClient.DeleteAsync("/api/products/" + id);
                }

                return new StatusCodeResult(200);
            }
            return new StatusCodeResult(404);
        }
    }
}

