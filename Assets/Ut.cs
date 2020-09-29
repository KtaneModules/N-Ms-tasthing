using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using rnd = UnityEngine.Random;

static class Ut
{
    public static T[] NewArray<T>(params T[] array)
    {
        return array;
    }

    public static T[] NewArray<T>(int size, Func<int, T> initialiser)
    {
        var result = new T[size];
        for (int i = 0; i < size; i++)
            result[i] = initialiser(i);
        return result;
    }

    /// <summary>
    ///     Brings the elements of the given list into a random order.</summary>
    /// <typeparam name="T">
    ///     Type of elements in the list.</typeparam>
    /// <param name="list">
    ///     List to shuffle.</param>
    /// <returns>
    ///     The list operated on.</returns>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        for (int j = list.Count; j >= 1; j--)
        {
            int item = rnd.Range(0, j);
            if (item < j - 1)
            {
                var t = list[item];
                list[item] = list[j - 1];
                list[j - 1] = t;
            }
        }
        return list;
    }

    /// <summary>
    ///     Returns the index of the first element in this <paramref name="source"/> satisfying the specified <paramref
    ///     name="predicate"/>. If no such elements are found, returns <c>-1</c>.</summary>
    public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        if (predicate == null)
            throw new ArgumentNullException("predicate");
        int index = 0;
        foreach (var v in source)
        {
            if (predicate(v))
                return index;
            index++;
        }
        return -1;
    }

    /// <summary>
    ///     Turns all elements in the enumerable to strings and joins them using the specified <paramref
    ///     name="separator"/> and the specified <paramref name="prefix"/> and <paramref name="suffix"/> for each string.</summary>
    /// <param name="values">
    ///     The sequence of elements to join into a string.</param>
    /// <param name="separator">
    ///     Optionally, a separator to insert between each element and the next.</param>
    /// <param name="prefix">
    ///     Optionally, a string to insert in front of each element.</param>
    /// <param name="suffix">
    ///     Optionally, a string to insert after each element.</param>
    /// <param name="lastSeparator">
    ///     Optionally, a separator to use between the second-to-last and the last element.</param>
    /// <example>
    ///     <code>
    ///         // Returns "[Paris], [London], [Tokyo]"
    ///         (new[] { "Paris", "London", "Tokyo" }).JoinString(", ", "[", "]")
    ///
    ///         // Returns "[Paris], [London] and [Tokyo]"
    ///         (new[] { "Paris", "London", "Tokyo" }).JoinString(", ", "[", "]", " and ");</code></example>
    public static string JoinString<T>(this IEnumerable<T> values, string separator = null, string prefix = null, string suffix = null, string lastSeparator = null)
    {
        if (values == null)
            throw new ArgumentNullException("values");
        if (lastSeparator == null)
            lastSeparator = separator;

        using (var enumerator = values.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                return "";

            // Optimise the case where there is only one element
            var one = enumerator.Current;
            if (!enumerator.MoveNext())
                return prefix + one + suffix;

            // Optimise the case where there are only two elements
            var two = enumerator.Current;
            if (!enumerator.MoveNext())
            {
                // Optimise the (common) case where there is no prefix/suffix; this prevents an array allocation when calling string.Concat()
                if (prefix == null && suffix == null)
                    return one + lastSeparator + two;
                return prefix + one + suffix + lastSeparator + prefix + two + suffix;
            }

            StringBuilder sb = new StringBuilder()
                .Append(prefix).Append(one).Append(suffix).Append(separator)
                .Append(prefix).Append(two).Append(suffix);
            var prev = enumerator.Current;
            while (enumerator.MoveNext())
            {
                sb.Append(separator).Append(prefix).Append(prev).Append(suffix);
                prev = enumerator.Current;
            }
            sb.Append(lastSeparator).Append(prefix).Append(prev).Append(suffix);
            return sb.ToString();
        }
    }

}
