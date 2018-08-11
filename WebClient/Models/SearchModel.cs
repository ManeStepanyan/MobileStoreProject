using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class SearchModel
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Version { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceTo { get; set; }
        public int? RAM { get; set; }
        public int? RAMTo { get; set; }
        public int? Year { get; set; }
        public int? YearTo { get; set; }
        public double? Display { get; set; }
        public int? Battery { get; set; }
        public int? BatteryTo { get; set; }
        public int? Camera { get; set; }
        public int? CameraTo { get; set; }
        public double? Memory { get; set; }
        public double? MemoryTo { get; set; }
        public string Color { get; set; }
    }
}
