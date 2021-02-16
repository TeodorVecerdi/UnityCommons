using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommons {
    public static partial class Extensions {
        #region Misc

        /// <summary>
        /// Returns the number of items in <paramref name="list"/>, or 0 if it is null
        /// </summary>
        public static int CountAllowNull<T>(this IList<T> list) {
            if (list != null)
                return list.Count;
            return 0;
        }

        /// <summary>
        /// Returns true if <paramref name="list"/> is empty or null, and false otherwise
        /// </summary>
        public static bool NullOrEmpty<T>(this IList<T> list) {
            if (list != null)
                return list.Count == 0;
            return true;
        }

        /// <summary>
        /// Shuffles <paramref name="list"/>
        /// </summary>
        public static void Shuffle<T>(this IList<T> list) {
            var count = list.Count;
            while (count > 1) {
                --count;
                var index = Rand.RangeInclusive(0, count);
                var obj = list[index];
                list[index] = list[count];
                list[count] = obj;
            }
        }

        /// <summary>
        /// Runs <paramref name="action"/> on each element of <paramref name="enumerable"/>
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            using (var enumerator = enumerable.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var current = enumerator.Current;
                    if (current == null) return;

                    action(current);
                }
            }
        }

        /// <summary>
        /// Joins <paramref name="enumerable"/> to <paramref name="source"/> and pipes it to an IEnumerable
        /// </summary>
        public static IEnumerable<T> AppendMany<T>(this IEnumerable<T> source, IEnumerable<T> enumerable) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            foreach (var element in source) yield return element;
            foreach (var element in enumerable) yield return element;
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> DeepCopyStruct<T>(this IEnumerable<T> source) where T : struct {
            var list = new List<T>();
            foreach (var item in source) {
                var copy = item;
                list.Add(copy);
            }

            return list;
        }

        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> DeepCopyStructOrNull<T>(this IEnumerable<T> source) where T : struct {
            return source?.DeepCopyStruct();
        }
        
        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> DeepCopyCloneable<T>(this IEnumerable<T> source) where T : ICloneable {
            return source.Select(item => (T) item.Clone()).ToList();
        }

        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> DeepCopyCloneableOrNull<T>(this IEnumerable<T> source) where T : ICloneable {
            return source?.DeepCopyCloneable();
        }

        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> ShallowCopy<T>(this IEnumerable<T> source) {
            return new List<T>(source);
        }

        /// <summary>
        /// Returns a deep copy of <paramref name="source"/>
        /// </summary>
        public static IList<T> ShallowCopyOrNull<T>(this IEnumerable<T> source) {
            return source?.ShallowCopy();
        }

        #endregion

        #region Duplicates

        /// <summary>
        /// Removes duplicates from <paramref name="list"/>
        /// </summary>
        public static void RemoveDuplicatesReference<T>(this List<T> list) where T : class {
            // Check if this `v` is faster than the current solution
            // list = new HashSet<T>(list).ToList();
            if (list.Count <= 1)
                return;
            for (var index1 = list.Count - 1; index1 >= 0; --index1)
            for (var index2 = 0; index2 < index1; ++index2)
                if (list[index1] == list[index2]) {
                    list.RemoveAt(index1);
                    break;
                }
        }

        /// <summary>
        /// Removes duplicates from <paramref name="list"/>
        /// </summary>
        public static void RemoveDuplicates<T>(this List<T> list) where T : IComparable<T> {
            if (list.Count <= 1)
                return;
            for (var index1 = list.Count - 1; index1 >= 0; --index1)
            for (var index2 = 0; index2 < index1; ++index2)
                if (list[index1].CompareTo(list[index2]) == 0) {
                    list.RemoveAt(index1);
                    break;
                }
        }

        /// <summary>
        /// Removes duplicates from <paramref name="list"/>
        /// </summary>
        public static void RemoveDuplicates<T>(this List<T> list, Comparison<T> comparison) {
            if (list.Count <= 1)
                return;
            for (var index1 = list.Count - 1; index1 >= 0; --index1)
            for (var index2 = 0; index2 < index1; ++index2)
                if (comparison(list[index1], list[index2]) == 0) {
                    list.RemoveAt(index1);
                    break;
                }
        }

        #endregion

        #region Printing

        /// <summary>
        /// Prints each element of <paramref name="list"/> with the format of "[a, b, c, d, ...]"
        /// </summary>
        public static void Print<T>(this IEnumerable<T> list, string separator = ", ", string start = "[", string end = "]") {
            Debug.Log(list.ToListString());
        }

        /// <summary>
        /// Joins each element of <paramref name="list"/> to a string with the format of "[a, b, c, d, ...]"
        /// </summary>
        public static string ToListString<T>(this IEnumerable<T> list, string separator = ", ", string start = "[", string end = "]") {
            return start + string.Join(separator, list.Select(item => item.ToString())) + end;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Returns true if <paramref name="list"/> is sorted, false otherwise
        /// </summary>
        public static bool IsSorted<T>(this IList<T> list, Comparison<T> comparison) {
            for (var i = 0; i < list.Count - 1; i++) {
                if (comparison(list[i], list[i + 1]) > 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if <paramref name="list"/> is sorted, false otherwise
        /// </summary>
        public static bool IsSorted<T>(this IList<T> list) where T : IComparable<T> {
            return IsSorted(list, (comparable, comparable1) => comparable.CompareTo(comparable1));
        }

        /// <summary>
        /// Returns <paramref name="enumerable"/> sorted using a quicksort algorithm
        /// </summary>
        public static IEnumerable<T> QuickSorted<T>(this IEnumerable<T> enumerable, Comparison<T> comparison) {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));

            var list = enumerable.ToList();
            QuickSort_Impl(list, 0, list.Count - 1, comparison);
            return list;
        }

        /// <summary>
        /// Returns <paramref name="enumerable"/> sorted using a quicksort algorithm
        /// </summary>
        public static IEnumerable<T> QuickSorted<T>(this IEnumerable<T> enumerable) where T : IComparable<T> {
            return QuickSorted(enumerable, (comparable, comparable1) => comparable.CompareTo(comparable1));
        }

        /// <summary>
        /// Returns <paramref name="enumerable"/> sorted using an insertion sorting algorithm
        /// </summary>
        public static IEnumerable<T> InsertionSorted<T>(this IEnumerable<T> enumerable, Comparison<T> comparison) {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));

            var list = enumerable.ToList();
            InsertionSort(list, comparison);
            return list;
        }

        /// <summary>
        /// Returns <paramref name="enumerable"/> sorted using an insertion sorting algorithm
        /// </summary>
        public static IEnumerable<T> InsertionSorted<T>(this IEnumerable<T> enumerable) where T : IComparable<T> {
            return InsertionSorted(enumerable, (comparable, comparable1) => comparable.CompareTo(comparable1));
        }

        /// <summary>
        /// Sorts <paramref name="list"/> using a quicksort algorithm
        /// </summary>
        public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison) {
            QuickSort_Impl(list, 0, list.Count - 1, comparison);
        }

        /// <summary>
        /// Sorts <paramref name="list"/> using a quicksort algorithm
        /// </summary>
        public static void QuickSort<T>(this IList<T> list) where T : IComparable<T> {
            QuickSort_Impl(list, 0, list.Count - 1, (comparable, comparable1) => comparable.CompareTo(comparable1));
        }

        /// <summary>
        /// Sorts <paramref name="list"/> using an insertion sorting algorithm
        /// </summary>
        public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison) {
            var count = list.Count;
            for (var index1 = 1; index1 < count; ++index1) {
                var y = list[index1];
                int index2;
                for (index2 = index1 - 1; index2 >= 0 && comparison(list[index2], y) > 0; --index2)
                    list[index2 + 1] = list[index2];
                list[index2 + 1] = y;
            }
        }

        /// <summary>
        /// Sorts <paramref name="list"/> using an insertion sorting algorithm
        /// </summary>
        public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T> {
            InsertionSort(list, (comparable, comparable1) => comparable.CompareTo(comparable1));
        }

        #endregion

        #region private Utilities

        private static void QuickSort_Impl<T>(this IList<T> list, int startIndex, int endIndex, Comparison<T> comparison) {
            if (startIndex >= endIndex)
                return;

            var partitionIndex = QuickSort_Partition(list, startIndex, endIndex, comparison);

            QuickSort_Impl(list, startIndex, partitionIndex - 1, comparison);
            QuickSort_Impl(list, partitionIndex + 1, endIndex, comparison);
            
            
            /* TODO!: Check if this is correct
            */
            /*while (true) {
                if (startIndex >= endIndex) return;

                var partitionIndex = QuickSort_Partition(list, startIndex, endIndex, comparison);

                QuickSort_Impl(list, startIndex, partitionIndex - 1, comparison);
                startIndex = partitionIndex + 1;
            }*/
        }

        private static int QuickSort_Partition<T>(IList<T> list, int low, int high, Comparison<T> comparison) {
            var pivot = list[high];
            var lowIndex = low - 1;

            for (var j = low; j < high; j++)
                if (comparison(list[j], pivot) <= 0) {
                    lowIndex++;
                    var temp = list[lowIndex];
                    list[lowIndex] = list[j];
                    list[j] = temp;
                }

            var temp1 = list[lowIndex + 1];
            list[lowIndex + 1] = list[high];
            list[high] = temp1;

            return lowIndex + 1;
        }

        #endregion
    }
}