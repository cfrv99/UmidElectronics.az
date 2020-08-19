using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmidShop.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int CategoryId { get; set; }
        public int? StockAmount { get; set; }
        public string Shipping { get; set; }
        public DateTime CreatedDate { get; set; }
        public Category Category { get; set; }
        public List<ProductImages> ProductImages { get; set; }
    }
}
