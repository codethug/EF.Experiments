using EF.Data.Context;
using EF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Test.Helpers
{
    public static class PeopleHelper
    {
        public static Parent CreateParent(this Context context, string name)
        {
            var parent = context.Parents.Add(new Parent()
            {
                Name = name
            });
            context.Parents.Add(parent);
            return parent;
        }

        public static Child CreateChild(this Context context, string name, Parent parent1 = null, Parent parent2 = null)
        {
            var child = context.Children.Add(new Child()
            {
                Name = name,
                Parent1 = parent1,
                Parent2 = parent2
            });
            context.Children.Add(child);
            return child;
        }
    }
}
