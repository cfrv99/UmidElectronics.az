using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.Entities;
using UmidShop.Models;

namespace UmidShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext context;

        public ShopController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet("butun-mallar")]
        public IActionResult GetAll([FromQuery]FilterModel filterModel)
        {
            int limit = 20;
            var data = context.Products.Skip((filterModel.CurrentPage - 1) * limit).Take(limit).ToList();
            if (filterModel.OrderBy == "Desc" && filterModel.OrderByWith == "Time")
            {
                data = data.OrderByDescending(i => i.CreatedDate).ToList();
            }
            else
            {
                data = data.OrderBy(i => i.Price).ToList();
            }
            if (filterModel.StartPrice != null && filterModel.EndPrice != null)
            {
                data = data.Where(i => i.Price >= filterModel.StartPrice && i.Price <= filterModel.EndPrice).ToList();
            }

            return View(data);
        }
        public async Task<IActionResult> GetById(int? id)
        {
            ProductDetailsModel model = new ProductDetailsModel();
            model.AdditionalImage = new List<string>();
            var conn = context.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            string query = @$"SELECT p.*,pim.ImageUrl as AdditionalImage 
                            FROM Products p left join ProductImages pim on p.Id = pim.ProductId  
                            WHERE p.Id={id}";
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();
            DataTable dt = new DataTable();
            dt.Load(reader);
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
                }
            }
            return View(model);
        }

    }
}
