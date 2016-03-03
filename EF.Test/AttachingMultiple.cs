using System.Linq;
using System.Data.Entity;
using EF.Test.Helpers;
using EF.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF.Data.Context;
using System;
using FluentAssertions;
using System.Collections.Generic;
using EF.Test.Helpers;
using System.Text;
using System.Text.RegularExpressions;

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
            var numParentsBefore = context.Parents.Count();
            // Remove items from EF cache
            context.Entry(createdParent1).State = EntityState.Detached;
            context.Entry(createdChild).State = EntityState.Detached;
            // Log all commands sent to the database
            var dbCommands = new StringBuilder();

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
            // Prevent parents from being re-added to database
            context.Entry(child.Parent1).State = EntityState.Detached;
            context.Entry(child.Parent2).State = EntityState.Detached;
            // Start logging commands
            context.Database.Log = (command) => dbCommands.AppendLine(command);
            context.SaveChanges();
            // Remove the child from EF's cache
            context.Entry(child).State = EntityState.Detached;
            // Stop logging commands
            context.Database.Log = null;
            var sql = dbCommands.ToString();

            // Assert
            // Verify that Updates were only done to Children
            var numUpdates = sql.CountSubstrings("UPDATE");
            var numChildrenUpdates = sql.CountSubstrings("UPDATE [dbo].[Children]");
            numUpdates.Should().Be(numChildrenUpdates);
            // Verify that no Inserts were done
            sql.CountSubstrings("INSERT").Should().Be(0);
            // Verify the child record was updated
            var assertChild = context.Children.Single(c => c.Id == createdChild.Id);
            assertChild.Name.Should().Be("New Child Name");
            // Verify no parents were added
            var numParentsAfter = context.Parents.Count();
            numParentsBefore.Should().Be(numParentsAfter);
        }

    }
}
