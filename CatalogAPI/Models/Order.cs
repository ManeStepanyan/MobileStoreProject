﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogAPI.Models
{
    /// <summary>
    /// Order
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
    }
}
