﻿using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerOrderController : Controller
    {
        private readonly Repo<CustomerOrder> repo;

        public CustomerOrderController(Repo<CustomerOrder> repo)
        {
            this.repo = repo;
        }

        // GET: api/CustomerProduct
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllCustomerOrders");
            if (result == null)
                return new StatusCodeResult(204);

            return new JsonResult(result);
        }

        // GET: api/CustomerProduct/5
        [HttpGet("{id}", Name = "GetOrdersByCustomerId")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> GetOrdersByCustomerId(int id)
        {
            List<int> orderIds = new List<int>();
            List<Order> orders = new List<Order>();
            int customerId;
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage resp = await customerClient.GetAsync("/api/customers/users/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await resp.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                customerId = customer.Id;
            }
            if (id == customerId)
            {
                orderIds = (List<int>)await this.repo.ExecuteOperationAsync("GetOrdersByCustomerId", new[] { new KeyValuePair<string, object>("id", id) });
                if (orderIds.Count == 0)
                {
                    return new JsonResult("No orders");
                }
                using (var orderClient = InitializeClient("http://localhost:5005/"))
                {
                    foreach (var orderid in orderIds)
                    {
                        HttpResponseMessage response = await orderClient.GetAsync("/api/orders/" + orderid);
                        orders.Add((Order)((await response.Content.ReadAsAsync(typeof(Order)))));
                    }
                }
                return new JsonResult(orders);
            }
            return new StatusCodeResult(404);
        }
        // GET: api/CustomerProduct/5
        [HttpGet("seller/{id}", Name = "GetOrdersBySellerId")]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> GetOrdersSellerId(int id)
        {
            List<int> orderIds = new List<int>();
            List<int> catalogsIds = new List<int>();
            List<Order> orders = new List<Order>();
            int sellerId;
            var userId = GetCurrentUser();
            using (var sellerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage resp = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                SellerPublicInfo seller = (SellerPublicInfo)((await resp.Content.ReadAsAsync(typeof(SellerPublicInfo))));
                sellerId = seller.Id;
            }
            if (id == sellerId)
            {
                catalogsIds = (List<int>)await this.repo.ExecuteOperationAsync("GetCatalogsIdsBySellerId", new[] { new KeyValuePair<string, object>("id", id) });
                using (var orderClient = InitializeClient("http://localhost:5005/"))
                {
                    foreach (var catalogid in catalogsIds)
                    {
                        HttpResponseMessage response = await orderClient.GetAsync("/api/orders/" + catalogid);
                        var res = await response.Content.ReadAsAsync(typeof(Order));
                        if (res != null)
                        {
                            orders.Add((Order)(res));
                        }
                    }
                }
                return new JsonResult(orders);
            }
            return new StatusCodeResult(404);
        }
        // POST: api/CustomerProduct
        [HttpPost]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> Post([FromBody] JToken jsonbody)
        { // don't forget ro delete product

            int orderId, customerId, quantity, catalogId;
            using (var orderClient = InitializeClient("http://localhost:5005/"))
            {
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await orderClient.PostAsync("/api/orders/", content);
                orderId = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());
                response = await orderClient.GetAsync("/api/orders/quantity" + orderId);
                quantity = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());
                response = await orderClient.GetAsync("/api/orders/catalog/" + orderId);
                catalogId = (int)((await response.Content.ReadAsAsync(typeof(int))));
            }
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                customerId = customer.Id;
            }
            using (var productClient = InitializeClient("http://localhost:5002/"))
            {
                var content = new FormUrlEncodedContent(new[]
        {
             new KeyValuePair<string, string>("quantity", quantity.ToString())

        });
                await productClient.PutAsync("/api/products/quantity/" + catalogId, content);
            }
            await this.repo.ExecuteOperationAsync("AddCustomerOrder", new[] { new KeyValuePair<string, object>("customerId", customerId), new KeyValuePair<string, object>("orderId", orderId) });

            return new JsonResult(200);
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