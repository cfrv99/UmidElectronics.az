using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.Entities;
using UmidShop.Models;

namespace UmidShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext context;
        private readonly IHostingEnvironment environment;

        public AdminController(IHostingEnvironment environment, AppDbContext context)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult AddProduct()
        {
            return View(new AddProductModel());
        }
        [HttpPost]
        public IActionResult AddProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Files!=null&&model.IsMainFile!=null)
                {
                    string uploads = string.Empty;
                    
                    Product p = new Product
                    {
                        Name = model.Name,
                        Price = model.Price,
                        CategoryId = model.CategoryId,
                        Desc = model.Desc,
                        Model = model.Model,
                        Discount = model.Discount,
                        CreatedDate = DateTime.Now
                    };
                    var product = context.Products.Add(p).Entity;
                    context.SaveChanges();
                    foreach (var file in model.Files)
                    {
                        var filename = Guid.NewGuid().ToString() + file.FileName.ToString();
                        uploads = Path.Combine(environment.WebRootPath, "uploads");
                        var pilePath = Path.Combine(uploads, filename);
                        file.CopyTo(new FileStream(pilePath, FileMode.OpenOrCreate));
                        ProductImages images = new ProductImages
                        {
                            ProductId = product.Id,
                            ImageUrl = filename
                        };
                        context.ProductImages.Add(images);
                    }
                    context.SaveChanges();
                    var mainFileName = Guid.NewGuid().ToString() + "Main" + model.IsMainFile.FileName.ToString();
                    var mainFilePath = Path.Combine(uploads, mainFileName);
                    model.IsMainFile.CopyTo(new FileStream(mainFilePath, FileMode.OpenOrCreate));
                    var addedProduct = context.Products.FirstOrDefault(i => i.Id == product.Id);
                    addedProduct.ImageUrl = mainFileName;
                    context.SaveChanges();
                }
            }
            return View();
        }
    }
}
