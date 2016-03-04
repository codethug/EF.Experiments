using EF.Data.Context;
using EF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Test.Helpers
{
    public static class AddressHelper
    {
        public static Address CreateAddress(this Context context, int customerId, AddressType type, string country)
        {
            var address = new Address()
            {
                CustomerId = customerId,
                Type = type,
                Country = country,
                PostalCode = "01234"
            };
            context.Addresses.Add(address);
            return address;
        }
    }
}
