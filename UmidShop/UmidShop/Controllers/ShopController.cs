using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.ADOConfig;
using UmidShop.Entities;
using UmidShop.Models;

namespace UmidShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext context;
        private int productCount;
        public ShopController(AppDbContext context)
        {
            this.context = context;
            productCount = context.Products.Count();
        }
        [HttpGet]
        public IActionResult GetAll(int? categoryId,string orderBy,string orderByWith,decimal? startPrice,decimal? endPrice,
            int currentPage=1)
        {
            int limit = 20;

            var data = context.Products.ToList();

            if (categoryId !=null)
            {
                data = data.Where(i => i.CategoryId == categoryId).ToList();
            }
            if (orderBy == "Desc" && orderByWith == "Time")
            {
                data = data.OrderByDescending(i => i.CreatedDate).ToList();
            }
            else
            {
                data = data.OrderBy(i => i.Price).ToList();
            }
            if (startPrice != null && endPrice != null)
            {
                data = data.Where(i => i.Price >= startPrice && i.Price <= endPrice).ToList();
            }

            data = data.Skip((currentPage - 1) * limit).Take(limit).ToList();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)productCount / limit);
            ViewBag.CurrentPage = currentPage;
            ViewBag.OrderBy = orderBy;
            ViewBag.OrderByWith = orderByWith;
            ViewBag.StartPrice = startPrice;
            ViewBag.EndPrice = endPrice;
            ViewBag.CategoryId = categoryId;
            return View(data);
        }

        public async Task<IActionResult> GetById(int? id)
        {
            ProductDetailsModel model = new ProductDetailsModel();
            model.AdditionalImage = new List<string>();
            string query = @$"SELECT p.*,pim.ImageUrl as AdditionalImage 
                            FROM Products p left join ProductImages pim on p.Id = pim.ProductId  
                            WHERE p.Id={id}";
            var dt = await Databases.Select(query, context);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    model.Id = Convert.ToInt32(row["Id"]);
                    model.Name = Convert.ToString(row["Name"]);
                    model.Price = Convert.ToDecimal(row["Price"]);
                    model.Desc = row["Desc"].ToString();
                    model.ImageUrl = row["ImageUrl"].ToString();
                    model.AdditionalImage.Add(row["AdditionalImage"].ToString());
                    model.Model = row["Model"].ToString();
                    model.Shipping = row["Shipping"].ToString();
                    model.StockAmount = Convert.ToInt32(row["StockAmount"]);

                }
            }
            return View(model);
        }

    }
}
