using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsAPI.Models;

namespace ProductAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {

        private readonly Repo<Product> repository;


        public ProductsController(Repo<Product> repository)
        {
            this.repository = repository;

        }


        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.repository.ExecuteOperationAsync("GetAllProducts");

            if (result == null)
                return new StatusCodeResult(204);

            return Ok(result);
        }



        // GET: api/Products/5
        [HttpGet("{Id}", Name = "GetProductById")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await this.repository.ExecuteOperationAsync("GetProduct", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet("name/{Name}", Name = "GetProductByName")]
        public async Task<IActionResult> Get(string name)
        {
            var res = await this.repository.ExecuteOperationAsync("GetProductByName", new[] { new KeyValuePair<string, object>("name", name) });

            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        // GET: api/Products/5
        [HttpGet("search/{listOfparams}", Name = "GetListOfProducts")]
        public async Task<IActionResult> GetListOfProducts(List<int> listOfIds)
        {
            var products = new List<object>();
            foreach (var id in listOfIds)
            {
                var res = await Get(id);
                products.Add(res);
            }
            return Ok(products);
        }


        [HttpPost("search", Name = "Search")]
        public async Task<IActionResult> Search([FromBody]Product product,decimal? priceTo = null,int? RAMTo = null,int? yearTo = null, int? batteryTo = null, int? cameraTo = null, int? memoryTo = null)
        {
            List<Product> result = new List<Product>();
            var products = (IEnumerable<Product>)await this.repository.ExecuteOperationAsync("SearchProducts", new[]
            {
                new KeyValuePair<string, object>("name", product.Name),
                new KeyValuePair<string, object>("brand", product.Brand),
                new KeyValuePair<string, object>("version", product.Version),
                new KeyValuePair<string, object>("priceFrom", product.Price),
                new KeyValuePair<string, object>("priceTo", priceTo),
                new KeyValuePair<string, object>("ram", product.RAM),
                new KeyValuePair<string, object>("ramTo", RAMTo),
                new KeyValuePair<string, object>("year", product.Year),
                new KeyValuePair<string, object>("yearTo", yearTo),
                new KeyValuePair<string, object>("display", product.Display),
                new KeyValuePair<string, object>("battery", product.Battery),
                new KeyValuePair<string, object>("batteryTo",batteryTo),
                new KeyValuePair<string, object>("camera", product.Camera),
                new KeyValuePair<string, object>("cameraTo", cameraTo),
                new KeyValuePair<string, object>("memory", product.Memory),
                new KeyValuePair<string, object>("memoryTo", memoryTo),
                new KeyValuePair<string, object>("color", product.Color),
            });
            if (products == null)
            {
                return NotFound();
            }
            foreach (var prod in products)
            {
                await this.repository.ExecuteOperationAsync("IncreamentCount", new[] { new KeyValuePair<string, object>("id", prod.Id) });

            }

            return Ok(products);
        }
        [HttpGet("mostsearched")]
        public async Task<IActionResult> GetMostSearchedProduct()
        {
            var products = await this.repository.ExecuteOperationAsync("MostSearchedProducts");
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            var res = (int)await this.repository.ExecuteOperationAsync("CreateProduct", new[]
            {
              new KeyValuePair<string, object>("name", product.Name),
              new KeyValuePair<string, object>("brand", product.Brand),
              new KeyValuePair<string, object>("version", product.Version),
              new KeyValuePair<string, object>("price", product.Price),
              new KeyValuePair<string, object>("ram", product.RAM),
              new KeyValuePair<string, object>("year", product.Year),
              new KeyValuePair<string, object>("display", product.Display),
              new KeyValuePair<string, object>("battery", product.Battery),
              new KeyValuePair<string, object>("camera", product.Camera),
              new KeyValuePair<string, object>("image", product.Image),
              new KeyValuePair<string, object>("quantity", product.Quantity),
              new KeyValuePair<string, object>("memory", product.Memory),
              new KeyValuePair<string, object>("color", product.Color),
              new KeyValuePair<string, object>("description", product.Description)
           });
            var temp = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            return Ok(temp);
        }


        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Product product)
        {
            await this.repository.ExecuteOperationAsync("UpdateProduct", new[]
            {
                new KeyValuePair<string, object>("id", id),
                new KeyValuePair<string, object>("name", product.Name),
                new KeyValuePair<string, object>("brand", product.Brand),
                new KeyValuePair<string, object>("version", product.Version),
                new KeyValuePair<string, object>("price", product.Price),
                new KeyValuePair<string, object>("ram", product.RAM),
                new KeyValuePair<string, object>("year", product.Year),
                new KeyValuePair<string, object>("display", product.Display),
                new KeyValuePair<string, object>("battery", product.Battery),
                new KeyValuePair<string, object>("camera", product.Camera),
                new KeyValuePair<string, object>("image", product.Image),
                new KeyValuePair<string, object>("quantity", product.Quantity),
                new KeyValuePair<string, object>("memory", product.Memory),
                new KeyValuePair<string, object>("color", product.Color),
                new KeyValuePair<string, object>("desciption", product.Description)
            });

            return Ok(await this.repository.ExecuteOperationAsync("GetProduct", new[]
                                  { new KeyValuePair<string, object>("id", id) }));
        }
        [HttpPut("quantity/{id}")]
        public async Task<IActionResult> Put(int id, int orderedQuantity)
        {
            var res = await this.repository.ExecuteOperationAsync("UpdateQuantity", new[]
               {
                new KeyValuePair<string, object>("id", id),
                new KeyValuePair<string, object>("orderedQuantity", orderedQuantity)
            });
            if (res == null) return NotFound();
            return new StatusCodeResult(200);
        }
        [HttpPut("catalog/{id}")]
        public async Task<IActionResult> PutCatalogId(int id, int catalogId)
        {
            var res = await this.repository.ExecuteOperationAsync("UpdateCatalogId", new[]
               {
                new KeyValuePair<string, object>("id", id),
                new KeyValuePair<string, object>("catalogId", catalogId)
            });
            if (res == null) return NotFound();
            return new StatusCodeResult(200);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this.repository.ExecuteOperationAsync("DeleteProduct", new[] {
                new KeyValuePair<string, object>("id", id),
            });
            return new StatusCodeResult(200);
        }
    }
}


