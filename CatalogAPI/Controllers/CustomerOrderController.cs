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
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllCustomerOrders");
            if (result == null)
                return new StatusCodeResult(204);

            return new JsonResult(result);
        }

        // GET: api/CustomerProduct/5
        [HttpGet("{id}", Name = "GetOrdersByCustomerId")]
        public async Task<IActionResult> GetOrdersByCustomerId(int id)
        {
            var res = await this.repo.ExecuteOperationAsync("GetOrdersByCustomerId", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return new StatusCodeResult(404);
            }
            return new JsonResult(res);
        }

        // POST: api/CustomerProduct
        [HttpPost]
        [Authorize(Policy ="Customer")]
        public async Task<IActionResult> Post([FromBody] JToken jsonbody)
        { // don't forget ro delete product

            int orderId, customerId, quantity, productId;
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
                response = await orderClient.GetAsync("/api/products/id" + orderId);
                productId = (int)((await response.Content.ReadAsAsync(typeof(int))));
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
                await productClient.PutAsync("/api/products/quantity/" + productId,content);
            }
            await this.repo.ExecuteOperationAsync("AddCustomerOrder", new[] { new KeyValuePair<string, object>("customerId", customerId), new KeyValuePair<string, object>("orderId", orderId) });

            return new JsonResult(200);
        }

   
    }
}