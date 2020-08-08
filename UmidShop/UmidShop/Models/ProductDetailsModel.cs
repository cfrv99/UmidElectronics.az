using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmidShop.Models
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public List<string> AdditionalImage { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
    }
}
