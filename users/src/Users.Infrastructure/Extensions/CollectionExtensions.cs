using System;
using System.Collections.Generic;
using System.Linq;

namespace Users.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static void Remove<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            var item = source.First(predicate);
            source.Remove(item);
        }
    }
}
