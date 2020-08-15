using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.Entities;

namespace UmidShop.Componnts
{
    public class CategoryList : ViewComponent
    {
        private readonly AppDbContext context;

        public CategoryList(AppDbContext context)
        {
            this.context = context;
        }
        public IViewComponentResult Invoke()
        {
            var categoryList = context.Categories.ToList();
            return View(categoryList);
        }
    }
}
