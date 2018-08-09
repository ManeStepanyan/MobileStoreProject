using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClient.Controllers
{
    public class OrderController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        public OrderController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: /<controller>/
        public IActionResult Order()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            // Make an API call
            var payment = Payment.Create(apiContext, new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "Transaction description.",
                        invoice_number = "001",
                        amount = new Amount
                        {
                            currency = "USD",
                            total = "100.00",
                            details = new Details
                            {
                                tax = "15",
                                shipping = "10",
                                subtotal = "75"
                            }
                        },
                        item_list = new ItemList
                        {
                            items = new List<Item>
                            {
                                new Item
                                {
                                    name = "Item Name",
                                    currency = "USD",
                                    price = "15",
                                    quantity = "5",
                                    sku = "sku"
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = "http://lockalhost:5004/Home/OrderSuccsseded",
                    cancel_url = "http://lockalhost:5004/Order/Index"
                }
            });
            return null;    
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
