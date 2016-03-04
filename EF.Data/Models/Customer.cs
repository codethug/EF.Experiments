using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(256)]
        public string Name { get; set; }

        private ICollection<Address> _addresses;
        protected virtual ICollection<Address> Addresses
        {
            get { return _addresses ?? (_addresses = new Collection<Address>()); }
            set { _addresses = value; }
        }

        [NotMapped]
        public Address DeliveryAddress
        {
            get { return GetAddress(AddressType.Delivery); }
        }

        private Address GetAddress(AddressType type)
        {
            return Addresses.FirstOrDefault(a => a.Type == type);
        }

        public static Expression<Func<Customer, ICollection<Address>>> AddressesAccessor = f => f.Addresses;
    }
}
