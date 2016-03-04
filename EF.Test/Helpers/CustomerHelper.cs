using EF.Data.Context;
using EF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Test.Helpers
{
    public static class CustomerHelper
    {
        public static Customer CreateCustomer(this Context context, string name)
        {
            var customer = new Customer()
            {
                Name = name
            };
            context.Customers.Add(customer);
            return customer;
        }
    }
}
