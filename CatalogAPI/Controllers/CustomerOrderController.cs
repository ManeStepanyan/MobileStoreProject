using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Post([FromBody] CustomerOrder customerOrder)
        { // don't forget ro delete product
            var res = await this.repo.ExecuteOperationAsync("AddCustomerOrder", new[]
            {
                new KeyValuePair<string, object>("ProductId", customerOrder.OrderId),
                new KeyValuePair<string, object>("CustomerId", customerOrder.CustomerId)
            });
            return new JsonResult(res);
        }

   
    }
}