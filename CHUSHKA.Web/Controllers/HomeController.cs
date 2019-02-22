using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CHUSHKA.Web.Models;
using CHUSHKA.Services.Contracts;
using CHUSHKA.Web.Models.Products;

namespace CHUSHKA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService productsService;

        public HomeController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            var productsTemp = this.productsService.GetAll();

            var products = new List<ProductInfoViewModel>();

            foreach (var pr in productsTemp)
            {
                products.Add(new ProductInfoViewModel
                {
                    Id = pr.Id,
                    Name = pr.Name,
                    Price = pr.Price,
                    Description = pr.Description.Length > 50 ? 
                                  pr.Description.Substring(0, 50) + "..." :
                                  pr.Description
                });
            }


            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
