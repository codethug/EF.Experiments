using System.Linq;
using System.Data.Entity;
using EF.Test.Helpers;
using EF.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF.Data.Context;
using System;
using FluentAssertions;

namespace EF.Test
{
    [TestClass]
    public class AttachingMultiple : TransactionTest
    {
        [TestMethod]
        public void AttachingEntityWithSameRelatedEntityTwice_ShouldThrowException()
        {
            // Arrange
            // Create items and save
            var createdParent1 = context.CreateParent("Susan");
            Child createdChild = context.CreateChild("Sam", createdParent1, createdParent1);
            context.SaveChanges();
            // Remove items from EF cache
            context.Entry(createdParent1).State = EntityState.Detached;
            context.Entry(createdChild).State = EntityState.Detached;

            // Act
            var child = context.Children
                            .AsNoTracking()
                            .Include(c => c.Parent1)
                            .Include(c => c.Parent2)
                            .Where(c => c.Id == createdChild.Id)
                            .Single();
            child.Name = "New Child Name";

            Action act = () => context.Children.Attach(child);

            // Assert
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Attaching an entity of type 'EF.Data.Models.Parent' failed because another " + 
                "entity of the same type already has the same primary key value. This can " + 
                "happen when using the 'Attach' method or setting the state of an entity to " + 
                "'Unchanged' or 'Modified' if any entities in the graph have conflicting key " + 
                "values. This may be because some entities are new and have not yet received " + 
                "database-generated key values. In this case use the 'Add' method or the 'Added' " + 
                "entity state to track the graph and then set the state of non-new entities " + 
                "to 'Unchanged' or 'Modified' as appropriate.");
        }

        [TestMethod]
        public void AddingEntityWithSameRelatedEntityTwice_ThenSettingToModifiedAndSaving_ShouldSaveData()
        {
            // Arrange
            // Create items and save
            var createdParent1 = context.CreateParent("Susan");
            Child createdChild = context.CreateChild("Sam", createdParent1, createdParent1);
            context.SaveChanges();
            // Remove items from EF cache
            context.Entry(createdParent1).State = EntityState.Detached;
            context.Entry(createdChild).State = EntityState.Detached;

            // Act
            var child = context.Children
                            .AsNoTracking()
                            .Include(c => c.Parent1)
                            .Include(c => c.Parent2)
                            .Where(c => c.Id == createdChild.Id)
                            .Single();
            child.Name = "New Child Name";

            context.Children.Add(child);
            context.Entry(child).State = EntityState.Modified;
            context.SaveChanges();
            // Remove the child from EF's cache
            context.Entry(child).State = EntityState.Detached;

            // Assert
            var assertChild = context.Children.Single(c => c.Id == createdChild.Id);
            assertChild.Name.Should().Be("New Child Name");
        }

    }
}
