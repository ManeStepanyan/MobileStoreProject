﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogAPI.Models
{
    /// <summary>
    /// Seller and Product correspondence
    /// </summary>
    public class SellerProduct
    {        
        public int ProductId { get; set; }
        public int SellerId { get; set; }
    }
}
