using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHUSHKA.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ChushkaDbContext context;

        public OrdersService(ChushkaDbContext context)
        {
            this.context = context;
        }

        public ICollection<Order> All()
        {
            return this.context
                .Orders
                .Include(x => x.Client)
                .Include(x => x.Product)
                .ToList();
        }

        public void Order(int productId, string username)
        {
            var user = this.context.Users
                .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new ArgumentException("User not found!");
            }

            var product = this.context.Products.Find(productId);

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

            var order = new Order
            {
                Client = user,
                Product = product,
                OrderedOn = DateTime.Now
            };

            this.context.Orders.Add(order);
            this.context.SaveChanges();

        }
    }
}
