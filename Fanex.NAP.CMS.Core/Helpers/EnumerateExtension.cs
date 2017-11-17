using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fanex.NAP.CMS.Core.Helpers
{
    public static class EnumerateExtension
    {
        public static Tuple<IEnumerable<T>, IEnumerable<T>> Split<T>(this IEnumerable<T> source, int count)
        {
            var c = 0;
            var fst = new Queue<T>();
            var snd = new Queue<T>();

            foreach (var item in source)
            {
                if (c < count)
                {
                    fst.Enqueue(item);
                }
                else
                {
                    snd.Enqueue(item);
                }
                ++c;
            }

            return new Tuple<IEnumerable<T>, IEnumerable<T>>(fst, snd);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);

            return source.OrderBy(x => propInfo.GetValue(x, null));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);

            return source.OrderByDescending(x => propInfo.GetValue(x, null));
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
#pragma warning disable S1226 // Method parameters and caught exceptions should not be reassigned
                source = source.Skip(chunksize);
#pragma warning restore S1226 // Method parameters and caught exceptions should not be reassigned
            }
        }
    }
}