using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.Entities;

namespace UmidShop.Componnts
{
    public class Slider : ViewComponent
    {
        private readonly AppDbContext context;

        public Slider(AppDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = context.Categories.ToList();
            return View(data);
        }
    }
}
