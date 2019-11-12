using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stein.Utility
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Iterates the <paramref name="source"/> and executes the given <paramref name="action"/> on every item.
        /// </summary>
        /// <typeparam name="T">Type of an item of <paramref name="source"/>.</typeparam>
        /// <param name="source">The enumeration to iterate.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on every item of <paramref name="source"/>.</param>
        /// <seealso cref="Apply{T}(IEnumerable{T}, Action{T})"/>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in source)
                action(item);
        }

        /// <summary>
        /// Iterates the <paramref name="source"/> and executes the given <paramref name="action"/> on every item.
        /// </summary>
        /// <param name="source">The enumeration to iterate.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on every item of <paramref name="source"/>.</param>
        /// <seealso cref="Apply(IEnumerable, Action{object})"/>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ForEach(this IEnumerable source, Action<object> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// <para>
        /// Performs a given <see cref="Action{T}"/> on every item of <paramref name="source"/>. 
        /// </para>
        /// </summary>
        /// <remarks>
        /// The execution is lazy to enable chaining of multiple statements. 
        /// Multiple iteration of the returned enumeration could result in multiple executions of <paramref name="action"/> on each item.
        /// </remarks>
        /// <typeparam name="T">Type of an item of <paramref name="source"/>.</typeparam>
        /// <param name="source">The enumeration to iterate.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on every item of <paramref name="source"/>.</param>
        /// <returns><paramref name="source"/> after the <paramref name="action"/> has been performed on every item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        /// <seealso cref="ForEach{T}(IEnumerable{T}, Action{T})"/>
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// <para>
        /// Performs a given <see cref="Action{T}"/> on every item of <paramref name="source"/>. 
        /// </para>
        /// </summary>
        /// <remarks>
        /// The execution is lazy to enable chaining of multiple statements. 
        /// Multiple iteration of the returned enumeration could result in multiple executions of <paramref name="action"/> on each item.
        /// </remarks>
        /// <param name="source">The enumeration to iterate.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on every item of <paramref name="source"/>.</param>
        /// <returns><paramref name="source"/> after the <paramref name="action"/> has been performed on every item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        /// <seealso cref="ForEach(IEnumerable, Action{object})"/>
        public static IEnumerable Apply(this IEnumerable source, Action<object> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Returns an enumeration of elements that are distinct based on a key returned by the given <paramref name="keySelector"/>.
        /// </summary>
        /// <typeparam name="TSource">Type of an element of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">Type of the key used to compare two elements.</typeparam>
        /// <param name="source">The source enumeration.</param>
        /// <param name="keySelector">A selector to the key of an element that should be used to compare two elements.</param>
        /// <returns>An enumeration of elements that are distinct based on the given <paramref name="keySelector"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.GroupBy(keySelector).Select(ig => ig.First());
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements using a lambda function.
        /// </summary>
        /// <param name="first">An enumeration to compare to <paramref name="second" />.</param>
        /// <param name="second">An enumeration to compare to the first sequence.</param>
        /// <param name="comparer">A <see cref="Func{T, T, TResult}" /> to compare the elements.</param>
        /// <typeparam name="TFirst">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second sequence.</typeparam>
        /// <returns>
        /// <see langword="true" /> if the two source sequences are of equal length and their corresponding elements compare equal according to <paramref name="comparer" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="first" />, <paramref name="second" /> or <paramref name="comparer" /> is <see langword="null" />.</exception>
        public static bool SequenceEqual<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            using var firstEnumerator = first.GetEnumerator();
            using var secondEnumerator = second.GetEnumerator();
            var firstHasNext = firstEnumerator.MoveNext();
            var secondHasNext = secondEnumerator.MoveNext();
            while (firstHasNext && secondHasNext)
            {
                if (!comparer.Invoke(firstEnumerator.Current, secondEnumerator.Current))
                    return false;
                firstHasNext = firstEnumerator.MoveNext();
                secondHasNext = secondEnumerator.MoveNext();
            }
            return !firstHasNext && !secondHasNext;
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements using a lambda function.
        /// </summary>
        /// <param name="first">An enumeration to compare to <paramref name="second" />.</param>
        /// <param name="second">An enumeration to compare to the first sequence.</param>
        /// <param name="comparer">A <see cref="Func{T, T, TResult}" /> to compare the elements.</param>
        /// <returns>
        /// <see langword="true" /> if the two source sequences are of equal length and their corresponding elements compare equal according to <paramref name="comparer" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="first" />, <paramref name="second" /> or <paramref name="comparer" /> is <see langword="null" />.</exception>
        public static bool SequenceEqual(this IEnumerable first, IEnumerable second, Func<object, object, bool> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            try
            {
                var firstHasNext = firstEnumerator.MoveNext();
                var secondHasNext = secondEnumerator.MoveNext();
                while (firstHasNext && secondHasNext)
                {
                    if (!comparer.Invoke(firstEnumerator.Current, secondEnumerator.Current))
                        return false;
                    firstHasNext = firstEnumerator.MoveNext();
                    secondHasNext = secondEnumerator.MoveNext();
                }
                return !firstHasNext && !secondHasNext;
            }
            finally
            {
                if (firstEnumerator is IDisposable firstDisposable)
                    firstDisposable.Dispose();
                if (secondEnumerator is IDisposable secondDisposable)
                    secondDisposable.Dispose();
            }
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements using a comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements in both sequences.</typeparam>
        /// <param name="first">An enumeration to compare to <paramref name="second" />.</param>
        /// <param name="second">An enumeration to compare to the first sequence.</param>
        /// <param name="comparer">An optional <see cref="IEqualityComparer{T}" /> to compare the elements.</param>
        /// <returns>If both sequences are equal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            using var firstEnumerator = first.GetEnumerator();
            using var secondEnumerator = second.GetEnumerator();
            var firstHasNext = firstEnumerator.MoveNext();
            var secondHasNext = secondEnumerator.MoveNext();
            while (firstHasNext && secondHasNext)
            {
                if (!(comparer ?? EqualityComparer<T>.Default).Equals(firstEnumerator.Current, secondEnumerator.Current))
                    return false;
                firstHasNext = firstEnumerator.MoveNext();
                secondHasNext = secondEnumerator.MoveNext();
            }
            return !firstHasNext && !secondHasNext;
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their elements using a comparer.
        /// </summary>
        /// <param name="first">An enumeration to compare to <paramref name="second" />.</param>
        /// <param name="second">An enumeration to compare to the first sequence.</param>
        /// <param name="comparer">An optional <see cref="IEqualityComparer" /> to compare the elements.</param>
        /// <returns>If both sequences are equal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
        public static bool SequenceEqual(this IEnumerable first, IEnumerable second, IEqualityComparer? comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            try
            {
                var firstHasNext = firstEnumerator.MoveNext();
                var secondHasNext = secondEnumerator.MoveNext();
                while (firstHasNext && secondHasNext)
                {
                    if (!(comparer ?? EqualityComparer<object>.Default).Equals(firstEnumerator.Current, secondEnumerator.Current))
                        return false;
                    firstHasNext = firstEnumerator.MoveNext();
                    secondHasNext = secondEnumerator.MoveNext();
                }
                return !firstHasNext && !secondHasNext;
            }
            finally
            {
                if (firstEnumerator is IDisposable firstDisposable)
                    firstDisposable.Dispose();
                if (secondEnumerator is IDisposable secondDisposable)
                    secondDisposable.Dispose();
            }
        }

        /// <summary>
        /// Iterates a given <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The enumeration to iterate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static void Iterate(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var item in source);
        }

        /// <summary>
        /// Continually loop over the given enumeration. If the given enumeration is empty, an empty enumeration is returned.
        /// </summary>
        /// <typeparam name="T">The type of items in the enumeration.</typeparam>
        /// <param name="source">The enumeration to loop over.</param>
        /// <returns>An enumeration containing the items from the given enumeration. After iterating the last item the next item will be the first item in the given enumeration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static IEnumerable<T> Loop<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!source.Any())
                yield break;

            while (true)
                foreach (var item in source)
                    yield return item;
        }

        /// <summary>
        /// Continually loop over the given enumeration. If the given enumeration is empty, an empty enumeration is returned.
        /// </summary>
        /// <param name="source">The enumeration to loop over.</param>
        /// <returns>An enumeration containing the items from the given enumeration. After iterating the last item the next item will be the first item in the given enumeration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static IEnumerable Loop(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!source.Any())
                yield break;

            while (true)
                foreach (var item in source)
                    yield return item;
        }

        /// <summary>
        /// Determines if the given enumeration contains any items.
        /// </summary>
        /// <param name="source">The enumeration to check if it contains any items.</param>
        /// <returns>If the given enumeration contains any items.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
        public static bool Any(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var enumerator = source.GetEnumerator();
            try
            {
                return enumerator.MoveNext();
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
