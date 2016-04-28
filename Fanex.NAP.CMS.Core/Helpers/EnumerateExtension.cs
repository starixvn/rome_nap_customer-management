namespace Fanex.NAP.CMS.Core.Helpers
{
    using System;
    using System.Collections.Generic;

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
    }
}