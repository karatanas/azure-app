using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CHUSHKA.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ChushkaDbContext context;

        public ProductsService(ChushkaDbContext context)
        {
            this.context = context;
        }

        public void Create(string name, decimal price, string description, string type)
        {
            ProductType productType = (ProductType)Enum.Parse(typeof(ProductType), type);

            var product = new Product
            {
                Name = name,
                Price = price,
                Type = productType,
                Description = description
            };

            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = this.context.Products.Find(id);

            this.context.Products.Remove(product);
            this.context.SaveChanges();
        }

        public bool Edit(int id, string name, decimal price, string description, string type)
        {
            var product = this.GetProduct(id);

            ProductType productType = (ProductType)Enum.Parse(typeof(ProductType), type);

            if (product == null)
            {
                return false;
            }

            product.Name = name;
            product.Price = price;
            product.Description = description;
            product.Type = productType;

            this.context.SaveChanges();

            return true;
        }

        public ICollection<Product> GetAll()
        {
            return this.context.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            return this.context.Products.Find(id);
        }
    }
}
