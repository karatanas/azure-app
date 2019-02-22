using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CHUSHKA.Services
{
    public class UsersService : IUsersService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ChushkaDbContext context;

        public UsersService(SignInManager<User> signInManager, UserManager<User> userManager, ChushkaDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;

            this.InitializeRoles();
        }

        private void InitializeRoles()
        {
            if (!this.context.Roles.Any())
            {
                var roles = new List<IdentityRole>{
                    new IdentityRole{
                        Name = "ADMIN",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole{
                        Name="USER",
                        NormalizedName = "USER"
                    }
                };
                this.context.Roles.AddRange(roles);
                this.context.SaveChanges();
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                return false;
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

            return signInResult.Succeeded;
        }

        public async void Logout()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<bool> Register(string username, string password, string confirmPassword, string email, string fullName)
        {
            if (username == null || confirmPassword == null ||
                password == null || email == null || fullName == null)
            {
                return false;
            }

            if (password != confirmPassword)
            {
                return false;
            }

            var user = new User
            {
                UserName = username,
                Email = email,
                FullName = fullName
            };

            var userCreateResult = await this.userManager.CreateAsync(user, password);

            if (!userCreateResult.Succeeded)
            {
                return false;
            }

            IdentityResult addRoleResult = null;

            if (this.userManager.Users.Count() == 1)
            {
                addRoleResult = await this.userManager.AddToRoleAsync(user, "ADMIN");
            }
            else
            {
                addRoleResult = await this.userManager.AddToRoleAsync(user, "USER");
            }

            if (!addRoleResult.Succeeded)
            {
                return false;
            }
            
            return true;
        }
    }
}
