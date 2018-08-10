using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class SearchModel
    {

        public int Id { get; set; }
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
        public string Image { get; set; }
        public int? Quantity { get; set; }
        public double? Memory { get; set; }
        public double? MemoryTo { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return ($"Id = {this.Id}&Name = {this.Name}&Brand = {this.Brand}&Version = {this.Version}&Price = {this.Price}&PriceTo = {this.PriceTo}&RAM = {this.RAM}&RAMTo = {this.RAMTo}&Year = {this.Year}&YearTo = {this.YearTo}&Display = {this.Display}&Battery = {this.Battery}&BatteryTo = {this.BatteryTo}&Camera = {this.Camera}&CameraTo = {this.CameraTo}&Image = {this.Image}&Quantity = {this.Quantity}&Memory = {this.Memory}&Color = {this.Color}&Description = {this.Description}");
        }
    }
}
