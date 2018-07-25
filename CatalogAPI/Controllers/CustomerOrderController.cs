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
        [Authorize(Policy ="Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllCustomerOrders");
            if (result == null)
                return new StatusCodeResult(204);

            return new JsonResult(result);
        }

        // GET: api/CustomerProduct/5
        [HttpGet("{id}", Name = "GetOrdersByCustomerId")]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> GetOrdersByCustomerId(int id)
        {
            List<int> orderIds = new List<int>();
            List<Order> orders = new List<Order>();
            int customerId;
            var userId = int.Parse(
                          ((ClaimsIdentity)this.User.Identity).Claims
                          .Where(claim => claim.Type == "user_id").First().Value);
            using (var customerClient = new HttpClient())
            {
                customerClient.BaseAddress = new Uri("http://localhost:5001/");
                customerClient.DefaultRequestHeaders.Accept.Clear();
                customerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage resp = await customerClient.GetAsync("/api/customers/users/" + userId);
                CustomerPublicInfo customer = (CustomerPublicInfo)((await resp.Content.ReadAsAsync(typeof(CustomerPublicInfo))));
                customerId = customer.Id;
            }
            if (id == customerId)
            {
               orderIds= (List<int>) await this.repo.ExecuteOperationAsync("GetOrdersByCustomerId", new[] { new KeyValuePair<string, object>("id", id) });
                if (orderIds.Count==0)
                {
                    return new JsonResult("No orders");
                }
                using (var orderClient = new HttpClient())
                {
                    orderClient.BaseAddress = new Uri("http://localhost:5005/");
                    orderClient.DefaultRequestHeaders.Accept.Clear();
                    orderClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var orderid in orderIds)
                    {
                        HttpResponseMessage response = await orderClient.GetAsync("/api/orders/"+ orderid);
                       orders.Add((Order)((await response.Content.ReadAsAsync(typeof(Order)))));
                    }             
                }
                return new JsonResult(orders);
            } return new StatusCodeResult(404);
        }
       
        // POST: api/CustomerProduct
        [HttpPost]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> Post([FromBody] JToken jsonbody)
        { // don't forget ro delete product

            int orderId, customerId, quantity, catalogId;
            using (var orderClient = new HttpClient())
            {
                orderClient.BaseAddress = new Uri("http://localhost:5005/");
                orderClient.DefaultRequestHeaders.Accept.Clear();
                orderClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await orderClient.PostAsync("/api/orders/", content);
                orderId = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());
                response = await orderClient.GetAsync("/api/orders/quantity" +orderId);
                quantity = int.Parse((await response.Content.ReadAsAsync(typeof(int))).ToString());
                response = await orderClient.GetAsync("/api/orders/catalog/" + orderId);
                catalogId = (int)((await response.Content.ReadAsAsync(typeof(int))));
            }
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
                customerId = customer.Id;
            }
            using (var productClient = new HttpClient())
            {
                productClient.BaseAddress = new Uri("http://localhost:5002/");
                productClient.DefaultRequestHeaders.Accept.Clear();
                productClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
        {
             new KeyValuePair<string, string>("quantity", quantity.ToString())

        });
                await productClient.PutAsync("/api/products/quantity/" + catalogId,content);
            }
            await this.repo.ExecuteOperationAsync("AddCustomerOrder", new[] { new KeyValuePair<string, object>("customerId", customerId), new KeyValuePair<string, object>("orderId", orderId) });

            return new JsonResult(200);
        }

   
    }
}