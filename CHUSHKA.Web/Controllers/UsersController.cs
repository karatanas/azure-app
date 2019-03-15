using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHUSHKA.Services.Contracts;
using CHUSHKA.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CHUSHKA.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.usersService.Logout();

            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var result = this.usersService.Login(model.Username, model.Password);

            if (!result.Result)
            {
                return this.View();
            }

            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            var registerUserResult = this.usersService.Register(
                model.Username,
                model.Password,
                model.ConfirmPassword,
                model.Email,
                model.FullName);

            if (!registerUserResult.Result)
            {
                return this.View();
            }

            var loginViewModel = new LoginUserViewModel
            {
                Username = model.Username,
                Password = model.Password
            };

            return this.Login(loginViewModel);
        }

    }
}
