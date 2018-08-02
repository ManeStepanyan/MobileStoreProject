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
                return NotFound();

            return Ok(result);
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
                if (!resp.IsSuccessStatusCode) return NotFound();
                CustomerPublicInfo customer = (CustomerPublicInfo)((await resp.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                customerId = customer.Id;
            }
            if (id == customerId)
            {
                orderIds = (List<int>)await this.repo.ExecuteOperationAsync("GetOrdersByCustomerId", new[] { new KeyValuePair<string, object>("id", id) });
                if (orderIds.Count == 0)
                {
                    return Ok("No orders");
                }
                using (var orderClient = InitializeClient("http://localhost:5005/"))
                {
                    foreach (var orderid in orderIds)
                    {
                        HttpResponseMessage response = await orderClient.GetAsync("/api/orders/" + orderid);
                        if (!response.IsSuccessStatusCode) return NotFound();
                        orders.Add((Order)((await response.Content.ReadAsAsync(typeof(Order)))));
                    }
                }
                return Ok(orders);
            }
            return NotFound();
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
                if (!resp.IsSuccessStatusCode) return NotFound();
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
                        if (!response.IsSuccessStatusCode) return NotFound();
                        var res = await response.Content.ReadAsAsync(typeof(Order));
                        if (res != null)
                        {
                            orders.Add((Order)(res));
                        }
                    }
                }
                return Ok(orders);
            }
            return NotFound();
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
                if (!response.IsSuccessStatusCode) return NotFound();
                Int32.TryParse(((await response.Content.ReadAsAsync(typeof(int))).ToString()), out orderId);
                if (orderId == 0) return NotFound();
                response = await orderClient.GetAsync("/api/orders/quantity" + orderId);
                Int32.TryParse(((await response.Content.ReadAsAsync(typeof(int))).ToString()), out quantity);
                if (quantity == 0) return NotFound();
                response = await orderClient.GetAsync("/api/orders/catalog/" + orderId);
                Int32.TryParse(((await response.Content.ReadAsAsync(typeof(int)))).ToString(), out catalogId);
                if (catalogId == 0) return NotFound();
            }
            var userId = GetCurrentUser();
            using (var customerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await customerClient.GetAsync("/api/customers/users/" + userId);
                if (!response.IsSuccessStatusCode) return NotFound();
                CustomerPublicInfo customer = (CustomerPublicInfo)((await response.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                customerId = customer.Id;
            }
            using (var productClient = InitializeClient("http://localhost:5002/"))
            {
                var content = new FormUrlEncodedContent(new[]
        {
             new KeyValuePair<string, string>("quantity", quantity.ToString())

        });
                HttpResponseMessage response = await productClient.PutAsync("/api/products/quantity/" + catalogId, content);
                if (!response.IsSuccessStatusCode) return NotFound();
            }
            var res = await this.repo.ExecuteOperationAsync("AddCustomerOrder", new[] { new KeyValuePair<string, object>("customerId", customerId), new KeyValuePair<string, object>("orderId", orderId) });
            if (res == null) return NotFound();
            return Ok(200);
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