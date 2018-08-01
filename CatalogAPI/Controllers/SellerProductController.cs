﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                return NotFound();

            return Ok(result);
        }

        // GET: api/SellerProduct/5
        [HttpGet("products/{id}", Name = "GetProductsBySellerId")]
        public async Task<IActionResult> GetProductsBySellerId(int id)
        {
            List<Product> list = new List<Product>();
            var sellerProducts = (IEnumerable<SellerProduct>)(await this.repo.ExecuteOperationAsync("GetProductsBySellerId", new[] { new KeyValuePair<string, object>("id", id) }));

            using (var productClient = InitializeClient("http://localhost:5002/"))
            {
                foreach (var item in sellerProducts)
                {
                    HttpResponseMessage response = await productClient.GetAsync("/api/products/" + item.ProductId);
                    if (response.IsSuccessStatusCode)
                    {
                        list.Add((Product)(await response.Content.ReadAsAsync(typeof(Product))));
                    }
                    else return NotFound();
                }
            }

            return Ok(list);
        }
        [HttpGet("catalog/{id}", Name = "GetProductByCatalogId")]
        public async Task<IActionResult> GetProductsByCatalogId(int id)
        {
            Product product = new Product();
            int productId = (int)(await this.repo.ExecuteOperationAsync("GetProductByCatalogId", new[] { new KeyValuePair<string, object>("catalogId", id) }));

            using (var productClient = InitializeClient("http://localhost:5002/"))
            {
                HttpResponseMessage response = await productClient.GetAsync("/api/products/" + productId);
                if (response.IsSuccessStatusCode)
                {
                    product = (Product)(await response.Content.ReadAsAsync(typeof(Product)));
                }
                else return NotFound();
            }
            return Ok(product);
        }

        // GET: api/SellerProduct/5
        // returns seller by specified product id
        [HttpGet("seller/{id}", Name = "GetSellerByProductId")]
        public async Task<IActionResult> GetSellerId(int id)
        {
            Object res;
            var sellerId = (int)await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            using (var sellerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/" + sellerId);
                res = await response.Content.ReadAsStringAsync();
            }
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet("{id}", Name = "GetCatalogIdByProductId")]
        public async Task<IActionResult> GetCatalogIdByProductId(int id)
        {
            int catalogId = (int)await this.repo.ExecuteOperationAsync("GetByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            if (catalogId != 0)
            {
                return Ok(catalogId);
            }
            return NotFound();
        }


        // POST: api/SellerProduct
        [HttpPost]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Post([FromBody]JToken jsonbody)
        {
            int productId, sellerId, catalogId;
            var userId = GetCurrentUser();
            using (var productClient = this.InitializeClient("http://localhost:5002/"))
            {
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await productClient.PostAsync("/api/products/", content);
                productId = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());

                using (var sellerClient = this.InitializeClient("http://localhost:5001/"))
                {
                    HttpResponseMessage resp = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                    SellerPublicInfo seller = (SellerPublicInfo)((await resp.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                    sellerId = seller.Id;
                }
                catalogId = Convert.ToInt32(await this.repo.ExecuteOperationAsync("AddSellerProduct", new[] { new KeyValuePair<string, object>("productId", productId), new KeyValuePair<string, object>("sellerId", sellerId) }));
                content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("catalogId", catalogId.ToString()) });
                response = await productClient.PutAsync("/api/products/catalog/" + productId, content);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Put(int id, [FromBody]JToken jsonbody)
        {
            var userId = GetCurrentUser();
            var sellerId = (int)await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            int currentSellerId;

            using (var sellerClient = this.InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage resp = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                SellerPublicInfo seller = (SellerPublicInfo)((await resp.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                currentSellerId = seller.Id;
            }
            if (currentSellerId == sellerId)
            {
                using (var productClient = this.InitializeClient("http://localhost:5002/"))
                {
                    HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await productClient.PutAsync("/api/products/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return new StatusCodeResult(200);
                    }
                }
            }
            return NotFound();
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            int currentSellerId;
            var sellerId = (int)await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) });
            var userId = GetCurrentUser();
            using (var sellerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                SellerPublicInfo seller = (SellerPublicInfo)((await response.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                currentSellerId = seller.Id;
            }
            if (currentSellerId == sellerId)
            {

                await this.repo.ExecuteOperationAsync("DeleteSellerProduct", new[]
                {
                new KeyValuePair<string, object>("id", id)
            });
                using (var productClient = InitializeClient("http://localhost:5002/"))
                {
                    await productClient.DeleteAsync("/api/products/" + id);
                }

                return new StatusCodeResult(200);
            }
            return NotFound();
        }

        public HttpClient InitializeClient(string uri)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
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
