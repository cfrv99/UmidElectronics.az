using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmidShop.Models
{
    public class AddProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Model { get; set; }
        public string Desc { get; set; }
        public int CategoryId { get; set; }
        public IFormFile IsMainFile { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
