using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF.Test.Helpers;
using EF.Data.Models;
using System.Linq;
using System.Data.Entity;
using FluentAssertions;
using System.Text;

namespace EF.Test
{
    // http://stackoverflow.com/questions/35780779/entity-framework-how-to-include-protected-collection

    [TestClass]
    public class SubCollectionTests : TransactionTest
    {
        [TestMethod]
        public void IncludingSubsetOfAddressesAndSettingAsProperty_ShouldNotThrowAnException()
        {
            // Arrange
            var customer = context.CreateCustomer("customer 1");
            context.SaveChanges();
            var address1 = context.CreateAddress(customer.Id, AddressType.Mailing, "US");
            var address2 = context.CreateAddress(customer.Id, AddressType.Delivery, "UK");
            context.SaveChanges();
            // Log all commands sent to the database
            var dbCommands = new StringBuilder();
            // Start logging commands
            context.Database.Log = (command) => dbCommands.AppendLine(command);

            // Act
            var customersWithDeliveryAddress =
                context.Customers
                    .Include(Customer.AddressesAccessor)
                    .Where(c => c.Id == customer.Id)
                    .ToList();
            // Stop logging commands
            context.Database.Log = null;

            // Assert
            var sql = dbCommands.ToString();
            customer.Id.Should().NotBe(0);
            customersWithDeliveryAddress.Count.Should().Be(1);
            customersWithDeliveryAddress[0].Name.Should().Be("customer 1");
            customersWithDeliveryAddress[0].DeliveryAddress.Should().NotBeNull();
            customersWithDeliveryAddress[0].DeliveryAddress.Type.Should().Be(AddressType.Delivery);
            customersWithDeliveryAddress[0].DeliveryAddress.Country.Should().Be("UK");
        }
    }
}
