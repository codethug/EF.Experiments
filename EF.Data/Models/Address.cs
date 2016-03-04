using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Models
{
    public enum AddressType
    {
        Delivery,
        Mailing
    }

    public class Address
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public AddressType Type { get; set; }
        [Required, MaxLength(100)]
        public string Country { get; set; }
        [Required, MaxLength(20)]
        public string PostalCode { get; set; }
    }
}
