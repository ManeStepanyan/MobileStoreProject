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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CatalogAPI.Controllers
{
    /// <summary>
    /// defines relationship between seller and product
    /// </summary>
    [Produces("application/json")]
    [Route("api/sellerproduct")]
    public class SellerProductController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly Repo<SellerProduct> repo;

        /// <summary>
        /// Initializing new repository to access db and http context accessor
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="httpContextAccessor"></param>
        public SellerProductController(Repo<SellerProduct> repo, IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.repo = repo;
        }

        // GET: api/SellerProduct
        /// <summary>
        /// Getting all correspondence between Sellers And Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.repo.ExecuteOperationAsync("GetAllSellerProducts");
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET: api/SellerProduct/5
        /// <summary>
        /// Getting all products which were added by specified seller
        /// </summary>
        /// <param name="id"> seller id </param>
        /// <returns></returns>
        [HttpGet("products/{id}", Name = "GetProductsBySellerId")]
        public async Task<IActionResult> GetProductsBySellerId(int id)
        {
            int selid = 0;
            var list = new List<Product>();
            using (var sellerClient = InitializeClient("http://localhost:5001/"))
            {
                HttpResponseMessage response = await sellerClient.GetAsync("/api/sellers/users/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var temp = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                    Int32.TryParse(temp.ToString(), out selid);
                }
            }
            if (selid == 0) return NotFound();
            var sellerProducts = (IEnumerable<SellerProduct>)(await this.repo.ExecuteOperationAsync("GetProductsBySellerId", new[] { new KeyValuePair<string, object>("id", selid) }));
            if (sellerProducts == null) return NotFound();
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
        /// <summary>
        /// Getting product by given catalog id
        /// </summary>
        /// <param name="id">catalog id </param>
        /// <returns></returns>
        [HttpGet("catalog/{id}", Name = "GetProductByCatalogId")]
        public async Task<IActionResult> GetProductsByCatalogId(int id)
        {
            var product = new Product();
            Int32.TryParse((await this.repo.ExecuteOperationAsync("GetProductByCatalogId", new[] { new KeyValuePair<string, object>("catalogId", id) })).ToString(), out int productId);
            if (productId == 0)
                return NotFound();

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
            int sellerId;
            Int32.TryParse((await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) })).ToString(), out sellerId);
            if (sellerId == 0)
                return NotFound();

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
            int catalogId;
            Int32.TryParse((await this.repo.ExecuteOperationAsync("GetByProductId", new[] { new KeyValuePair<string, object>("id", id) })).ToString(), out catalogId);
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
                var content =(HttpContent) new StringContent(jsonbody.ToString(), Encoding.UTF8, "application/json");
                var response = await productClient.PostAsync("/api/products/", content); ;
                var temp = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                Int32.TryParse(temp.ToString(), out productId);
                if (productId == 0)
                    return NotFound();

                using (var sellerClient = this.InitializeClient("http://localhost:5001/"))
                {
                    var resp = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                    temp = JsonConvert.DeserializeObject<int>(await resp.Content.ReadAsStringAsync());
                    Int32.TryParse(temp.ToString(), out sellerId);
                    if (sellerId == 0) return NotFound();
                }
                catalogId = Convert.ToInt32(await this.repo.ExecuteOperationAsync("AddSellerProduct", new[] { new KeyValuePair<string, object>("productId", productId), new KeyValuePair<string, object>("sellerId", sellerId) }));
                content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("catalogId", catalogId.ToString()) });
                response = await productClient.PutAsync("/api/products/catalog/" + productId, content);
            }
            return Ok();
        }

       /// <summary>
       /// Deleting product
       /// </summary>
       /// <param name="id"> product id </param>
       /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            int currentSellerId, sellerId;
            Int32.TryParse((await this.repo.ExecuteOperationAsync("GetSellerByProductId", new[] { new KeyValuePair<string, object>("id", id) })).ToString(), out sellerId);
            if (sellerId == 0)
                return NotFound();
            var userId = GetCurrentUser();
            using (var sellerClient = InitializeClient("http://localhost:5001/"))
            {
                var response = await sellerClient.GetAsync("/api/sellers/users/" + userId);
                var temp = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                Int32.TryParse(temp.ToString(), out currentSellerId);
                if (currentSellerId == 0) return NotFound();
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

                return Ok();
            }
            return NotFound();
        }
        /// <summary>
        /// Initializing http client
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>

        public HttpClient InitializeClient(string uri)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            var authInfo = _httpContextAccessor.HttpContext.AuthenticateAsync();
            if (authInfo.Result.Properties != null)
            {
                var token = authInfo.Result.Properties.Items.Values.ElementAt(0);

                client.SetBearerToken(token);
            }
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
