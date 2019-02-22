using CHUSHKA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CHUSHKA.Services.Contracts
{
    public interface IProductsService
    {
        void Create(string name, decimal price, string description, string type);

        Product GetProduct(int id);

        ICollection<Product> GetAll();

        bool Edit(int id, string name, decimal price, string description, string type);

        void Delete(int id);
    }
}
