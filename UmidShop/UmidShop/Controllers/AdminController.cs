using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categoryList = new SelectList(context.Categories.ToList(), "Id", "Name");
            ViewBag.CategoryList = categoryList;
            return View(new AddProductModel());
        }
        [HttpPost]
        public IActionResult AddProduct(AddProductModel data)
        {
            if (ModelState.IsValid)
            {
                if (data.Files!=null&& data.IsMainFile!=null)
                {
                    string uploads = string.Empty;
                    
                    Product p = new Product
                    {
                        Name = data.Name,
                        Price = data.Price,
                        CategoryId = data.CategoryId,
                        Desc = data.Desc,
                        Model = data.Model,
                        Discount = data.Discount,
                        CreatedDate = DateTime.Now,
                        Shipping= data.Shipping,
                        StockAmount= data.StockAmount,
                        
                    };
                    var product = context.Products.Add(p).Entity;
                    context.SaveChanges();
                    foreach (var file in data.Files)
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
                    var mainFileName = Guid.NewGuid().ToString() + "Main" + data.IsMainFile.FileName.ToString();
                    var mainFilePath = Path.Combine(uploads, mainFileName);
                    data.IsMainFile.CopyTo(new FileStream(mainFilePath, FileMode.OpenOrCreate));
                    var addedProduct = context.Products.FirstOrDefault(i => i.Id == product.Id);
                    addedProduct.ImageUrl = mainFileName;
                    context.SaveChanges();
                }
                return RedirectToAction("GetAll");
            }
            var categoryList = new SelectList(context.Categories.ToList(), "Id", "Name");
            ViewBag.CategoryList = categoryList;
            return View(data);
        }
        public IActionResult GetAll()
        {
            var data = context.Products.ToList();
            return View(data);
        }
        public IActionResult Delete(int id)
        {
            
            var data = context.Products.FirstOrDefault(i => i.Id == id);
            context.Products.Remove(data);
            context.SaveChanges();
            return RedirectToAction("GetAll");
        }
        public IActionResult Edit(int id)
        {
            var categoryList = new SelectList(context.Categories.ToList(), "Id", "Name");
            ViewBag.CategoryList = categoryList;
            var data = context.Products.FirstOrDefault(i => i.Id == id);

            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (product.Id <= 0)
                return BadRequest();
            var data = context.Products.FirstOrDefault(i => i.Id == product.Id);
            data.StockAmount = product.StockAmount;
            data.Shipping = product.Shipping;
            data.Discount = product.Discount;
            context.SaveChanges();
            return RedirectToAction("GetAll");
        }
        public IActionResult GetAllCategories()
        {
            var data = context.Categories.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("GetAllCategories");
        }
        public IActionResult DeleteCategory(int id)
        {
            var data = context.Categories.FirstOrDefault(i => i.Id == id);
            context.Categories.Remove(data);
            context.SaveChanges();
            return RedirectToAction("GetAllCategories");
        }
        public IActionResult GetAllImages()
        {
            var data = context.Images.ToList();

            return View(data);
        }
        [HttpGet]
        public IActionResult CreateImages()
        {
            return View(new Images());
        }
        [HttpPost]
        public IActionResult CreateImages(Images images,IFormFile file)
        {
            var filename = Guid.NewGuid().ToString() + file.FileName.ToString();
            var uploads = Path.Combine(environment.WebRootPath, "uploads");

            var path = Path.Combine(uploads, filename);
            file.CopyTo(new FileStream(path, FileMode.OpenOrCreate));
            images.ImageUrl = filename;
            context.Images.Add(images);
            context.SaveChanges();
            return RedirectToAction("GetAllImages");
        }
    }
}
