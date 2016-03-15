using EF.Data.Context;
using EF.Data.Models;
using EF.Test.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Test
{
    [TestClass]
    public class ProxyMonitoringTests : TransactionTest
    {
        [TestMethod]
        public void EntityAddedAndSavedAndRetrieved_ShouldNotHaveProxyClass_WhileUsingSameContext()
        {
            var c = new Child() { Name = "Charlotte" };
            c.GetType().Name.Should().Be("Child");
            var c2 = context.Children.Add(c);
            c.GetType().Name.Should().Be("Child");
            c2.GetType().Name.Should().Be("Child");

            context.SaveChanges();
            c.GetType().Name.Should().Be("Child");
            c2.GetType().Name.Should().Be("Child");

            var c3 = context.Children.Single(ch => ch.Id == c.Id);
            c3.GetType().Name.Should().Be("Child");
        }

        [TestMethod]
        public void EntityAddedWithObjectCreate_ShouldHaveProxyClass_WhileUsingSameContext()
        {
            // For this test, the Proxy class wasn't initially being created until I added the 
            // virtual keyword for Parent1 and Parent2 on the Child class.  Documentation for 
            // what is needed for Proxy classes to be created can be found here:
            //    https://msdn.microsoft.com/library/dd468057(v=vs.100).aspx
            var c = context.Children.Create();
            c.Name = "Charlotte";
            c.GetType().Name.Should().Be("Child_E6959D97EAFBE4CEC5EC7D517A07684F07A3217C93A9B746FE98F774A9C39434");
        }

        [TestMethod]
        public void EntityAddedSaved_ShouldHaveProxyClass_WhenRetrievedWithNewContext()
        {
            // Arrange
            Child c5;
            using (var context2 = new Context())
            {
                c5 = new Child() { Name = "Susan" };
                context2.Children.Add(c5);
                context2.SaveChanges();
            }

            // Act
            var c6 = context.Children.Single(ch => ch.Id == c5.Id);

            // Assert
            c6.GetType().Name.Should().Be("Child_E6959D97EAFBE4CEC5EC7D517A07684F07A3217C93A9B746FE98F774A9C39434");
        }

    }
}
