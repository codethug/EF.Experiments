using System.Data.Entity;
using EF.Data.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.Test.Helpers
{
    [TestClass]
    public class TransactionTest
    {
        protected Context context;
        protected DbContextTransaction transaction;

        [TestInitialize]
        public void TransactionTestStart()
        {
            context = new Context();
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TransactionTestEnd()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }
    }
}
