using System.Data.Entity;
using EF.Data.Models;

namespace EF.Data.Context
{
    public class Context : DbContext
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
    }
}
