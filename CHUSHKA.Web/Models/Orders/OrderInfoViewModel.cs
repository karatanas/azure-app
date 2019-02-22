using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CHUSHKA.Web.Models.Orders
{
    public class OrderInfoViewModel
    {
        public int OrderId { get; set; }

        public string Username { get; set; }

        public string ProductName { get; set; }

        public string OrderedOn { get; set; }

    }
}
