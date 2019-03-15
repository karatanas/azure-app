using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Contracts;
using CHUSHKA.Web.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CHUSHKA.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ChushkaDbContext db;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            var types = Enum.GetNames(typeof(ProductType)).ToList();

            var model = new ProductCreateViewModel
            {
                Types = types
            };

            return this.View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.productsService.Create(
                 model.Name,
                 model.Price,
                 model.Description,
                 model.Type);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var product = this.productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductDetailsViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Type = product.Type.ToString(),
                Description = product.Description,
                Id = product.Id
            };


            return this.View(model);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int id)
        {
            var product = this.productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductCreateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString(),
                Id = product.Id
            };

            var types = Enum.GetNames(typeof(ProductType)).ToList();
            model.Types = types;

            return this.View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Edit(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/");
            }

            var isEdited = this.productsService.Edit(
                 model.Id,
                  model.Name,
                  model.Price,
                  model.Description,
                  model.Type);

            if (!isEdited)
            {
                return this.Redirect("/");
            }

            return this.RedirectToAction(nameof(Details), routeValues: model.Id);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            var product = this.productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductCreateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString(),
                Id = product.Id
            };

            var types = Enum.GetNames(typeof(ProductType)).ToList();
            model.Types = types;

            return this.View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Delete(ProductCreateViewModel model)
        {
            
            this.productsService.Delete(model.Id);

            return this.Redirect("/");
        }
        public async Task<IActionResult> Index(string searchString,
             int? page, string currentFilter, ProductSortState sortOrder = ProductSortState.NameAsc)//, 
        {
            var products = from s in db.Products
                           select s;


            ViewData["NameSort"] = sortOrder == ProductSortState.NameAsc ? ProductSortState.NameDesc : ProductSortState.NameAsc;
            ViewData["PriceSort"] = sortOrder == ProductSortState.PriceAsc ? ProductSortState.PriceDesc : ProductSortState.PriceAsc;


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
                
            }


            int pageSize = 3;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
            
        }
    }
}
