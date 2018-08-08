using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersAndShopCartAPI.Models
{
    public class ShopCart
    {
        public int CustomerId { get; set; }
        public int CatalogId { get; set; }
        public int Quantity { get; set; }
    }
}