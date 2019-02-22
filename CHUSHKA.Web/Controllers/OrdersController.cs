using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHUSHKA.Services.Contracts;
using CHUSHKA.Web.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CHUSHKA.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [Authorize]
        public IActionResult Order(int id)
        {
            this.ordersService.Order(id, this.User.Identity.Name);
            
            return Redirect("/");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult All()
        {
            var orders = this.ordersService.All();

            var orderViewModels = new List<OrderInfoViewModel>();

            foreach (var or in orders)
            {
                orderViewModels.Add(new OrderInfoViewModel
                {
                    ProductName = or.Product.Name,
                    Username = or.Client.UserName,
                    OrderedOn = or.OrderedOn.ToString(@"dd\/MM\/yyyy"),
                    OrderId = or.Id
                });
            }

            return View(orderViewModels);
        }
    }
}
