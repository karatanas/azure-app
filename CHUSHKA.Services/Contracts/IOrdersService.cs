using CHUSHKA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHUSHKA.Services.Contracts
{
    public interface IOrdersService
    {
        void Order(int productId,string username);

        ICollection<Order> All();

    }
}
