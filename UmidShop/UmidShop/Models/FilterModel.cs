using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmidShop.Models
{
    public class FilterModel
    {
        public string OrderBy { get; set; } = "Asc";
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public int CurrentPage { get; set; } = 1;
        public string OrderByWith { get; set; } = "Pricing";
        public int CategoryId { get; set; }
    }
}
