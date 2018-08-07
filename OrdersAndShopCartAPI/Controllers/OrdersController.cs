using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersAndShopCartAPI.Models;

namespace OrdersAndShopCartAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly Repo<Order> repo;

        public OrdersController(Repo<Order> repo)
        {
            this.repo = repo;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllOrders");
            if (result == null)
                return new StatusCodeResult(204);

            return Ok(result);
        }

        // GET: api/Orders/5
        [HttpGet("{id}", Name = "GetOrderByOrderId")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var res = await this.repo.ExecuteOperationAsync("GetOrderByOrderId", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        // GET: api/Orders/5
        [HttpGet("{date}", Name = "GetOrderByTimeSpan")]
        [Authorize(Policy ="Admin")]
        public async Task<IActionResult> GetOrderByTimeSpan(DateTime start, DateTime end)
        {
            var res = await this.repo.ExecuteOperationAsync("GetOrderByTimeSpan", new[]
            {
                new KeyValuePair<string, object>("start", start),
                new KeyValuePair<string, object>("end", end)
            });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpGet("quantity/{orderId}", Name = "GetQuantityByOrderId")]
        public async Task<IActionResult> GetQuantity(int orderId)
        {
            var res = await this.repo.ExecuteOperationAsync("GetQuantity", new[]
            {      new KeyValuePair<string, object>("orderId", orderId) });
            if (res == null)
            {
                return NotFound();
            } return Ok(res);
        }
        [HttpGet("catalog/{orderId}", Name = "GetCatalogByOrderId")]
        public async Task<IActionResult> GetCatalog(int orderId)
        {
            var res = await this.repo.ExecuteOperationAsync("GetCatalogIdByOrderId", new[]
            {      new KeyValuePair<string, object>("id", orderId) });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpGet("{id}", Name = "GetOrderByCatalogId")]
        public async Task<IActionResult> GetOrderByCatalogId(int id)
        {
            var res = await this.repo.ExecuteOperationAsync("GetOrderByCatalogId", new[]
            {
                new KeyValuePair<string, object>("Catalog_Id", id)
            });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            var res = await this.repo.ExecuteOperationAsync("CreateOrder", new[]
            {
                new KeyValuePair<string, object>("Catalog", order.CatalogId),
                new KeyValuePair<string, object>("Date", DateTime.Now),
                new KeyValuePair<string, object>("Address", order.Address),
                new KeyValuePair<string, object>("CellPhone", order.CellPhone),
                new KeyValuePair<string, object>("Quantity", order.Quantity),
                new KeyValuePair<string, object>("TotalAmount", order.TotalAmount),
                new KeyValuePair<string, object>("CardAccount", order.CardAccount)
            });
            return Ok(res);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           var res= await this.repo.ExecuteOperationAsync("DeleteOrder", new[] { new KeyValuePair<string, object>("id", id) });
            if(res!=null)
            return Ok();
            return NotFound();
        }
    }
}
