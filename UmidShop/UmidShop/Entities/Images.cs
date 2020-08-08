using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmidShop.Entities
{
    public class Images
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSlider { get; set; }
        public bool IsCover { get; set; }
    }
}
