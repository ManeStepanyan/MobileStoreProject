using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogAPI.Models
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
    }
}
