using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EF.Test.Helpers
{
    public static class StringExtensions
    {
        public static int CountSubstrings(this string input, string textToFind)
        {
            return Regex.Matches(input, Regex.Escape(textToFind)).Count;
        }
    }
}
