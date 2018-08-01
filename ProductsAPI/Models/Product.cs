using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Version { get; set; }
        public decimal? Price { get; set; }
        public int? RAM { get; set; }
        public int? Year { get; set; }
        public decimal? Display { get; set; }
        public int? Battery { get; set; }
        public int? Camera { get; set; }
        public string Image { get; set; }
        public int? Quantity { get; set; }
        public decimal? Memory { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
