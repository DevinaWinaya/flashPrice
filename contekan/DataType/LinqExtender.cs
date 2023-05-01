using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCFx.Extender.DataType
{
    /// <summary>
    /// LINQ Extenders
    /// Contains useful Linq Extenders
    /// 
    /// https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
    /// 
    /// Preferrable: https://github.com/morelinq/MoreLINQ
    /// 
    /// </summary>
    public static class LinqExtender
    {
        /// <summary>
        /// Performs filtering of IEnumerable instance by given keySelector function
        /// </summary>
        /// <typeparam name="TSource">The type of collection</typeparam>
        /// <typeparam name="TKey">The type of selector</typeparam>
        /// <param name="source">The source of IEnumerable Object</param>
        /// <param name="keySelector">The function to evaluate the uniqueness. i.e. The attribute of object</param>
        /// <returns>IEnumerable instance with distinct values of selected key</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Checks whether the specified <paramref name="item"/> available in <paramref name="source"/> with default <see cref="IEqualityComparer{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="item">The item to search in <param name="source"></param></param>
        /// <param name="source">The source to search <paramref name="item"/> from</param>
        /// <returns>True if the specified <paramref name="item"/> is in <paramref name="source"/></returns>
        public static bool IsIn<TSource>(this TSource item, IEnumerable<TSource> source)
        {
            return source.Contains(item);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="item"/> available in <paramref name="source"/> with specified <paramref name="comparer"/>
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="item">The item to search in <param name="source"></param></param>
        /// <param name="source">The source to search <paramref name="item"/> from</param>
        /// <param name="comparer">A <see cref="IEqualityComparer{TSource}"/> to check item availability</param>
        /// <returns>True if the specified <paramref name="item"/> is in <paramref name="source"/></returns>
        public static bool IsIn<TSource>(this TSource item, IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            return source.Contains(item, comparer);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="item"/> available in <paramref name="source"/> with default <see cref="IEqualityComparer{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="item">The item to search in <param name="source"></param></param>
        /// <param name="source">The source to search <paramref name="item"/> from</param>
        /// <returns>True if the specified <paramref name="item"/> is in <paramref name="source"/></returns>
        public static bool IsIn<TSource>(this TSource item, IQueryable<TSource> source)
        {
            return source.Contains(item);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="item"/> available in <paramref name="source"/> with specified <paramref name="comparer"/>
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="item">The item to search in <param name="source"></param></param>
        /// <param name="source">The source to search <paramref name="item"/> from</param>
        /// <param name="comparer">A <see cref="IEqualityComparer{TSource}"/> to check item availability</param>
        /// <returns>True if the specified <paramref name="item"/> is in <paramref name="source"/></returns>
        public static bool IsIn<TSource>(this TSource item, IQueryable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            return source.Contains(item, comparer);
        }

        /// <summary>
        /// Performs actions for each element in <see cref="IEnumerable{T}"/> instance with specified <see cref="Action{T}"/>
        /// </summary>
        /// <typeparam name="TSource">The element type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/> to perform action</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each element</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }

        /// <summary>
        /// Performs actions for each element in <see cref="IEnumerable{T}"/> instance with specified <see cref="Action{T}"/>
        /// while incorporating the element index for each elements
        /// </summary>
        /// <typeparam name="TSource">The element type of <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/> to perform action</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each element as well as holding the element index</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            int index = 0;
            foreach (var element in source)
            {
                action(element, index++);
            }
        }

        public static void AddRange<TElement>(this ICollection<TElement> source, IEnumerable<TElement> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
