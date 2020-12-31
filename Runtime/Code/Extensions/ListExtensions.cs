using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommons {
    public static class ListExtensions {
        #region Misc
        public static int CountAllowNull<T>(this IList<T> list) {
            if (list != null)
                return list.Count;
            return 0;
        }

        public static bool NullOrEmpty<T>(this IList<T> list) {
            if (list != null)
                return list.Count == 0;
            return true;
        }
        
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
#endregion

        #region Copy
        public static List<T> DeepCopyStruct<T>(this IEnumerable<T> source) where T : struct {
            var list = new List<T>();
            foreach (var item in source) {
                var copy = item;
                list.Add(copy);
            }

            return list;
        }

        public static List<T> DeepCopyStructOrNull<T>(this IEnumerable<T> source) where T : struct {
            return source?.DeepCopyStruct();
        }
        
        public static List<T> DeepCopyCloneable<T>(this IEnumerable<T> source) where T : ICloneable {
            return source.Select(item => (T) item.Clone()).ToList();
        }

        public static List<T> DeepCopyCloneableOrNull<T>(this IEnumerable<T> source) where T : ICloneable {
            return source?.DeepCopyCloneable();
        }

        public static List<T> ShallowCopy<T>(this IEnumerable<T> source) {
            return new List<T>(source);
        }

        public static List<T> ShallowCopyOrNull<T>(this IEnumerable<T> source) {
            return source?.ShallowCopy();
        }
        #endregion

        #region Duplicates
        public static void RemoveDuplicatesReference<T>(this List<T> list) where T : class {
            if (list.Count <= 1)
                return;
            for (var index1 = list.Count - 1; index1 >= 0; --index1)
            for (var index2 = 0; index2 < index1; ++index2)
                if (list[index1] == list[index2]) {
                    list.RemoveAt(index1);
                    break;
                }
        }
        
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
        public static void RemoveDuplicates<T>(this List<T> list, Comparison<T> comparison) {
            if (list.Count <= 1)
                return;
            for (var index1 = list.Count - 1; index1 >= 0; --index1)
            for (var index2 = 0; index2 < index1; ++index2)
                if (comparison(list[index1],list[index2]) == 0) {
                    list.RemoveAt(index1);
                    break;
                }
        }
        #endregion
        
        #region Printing
        public static void Print<T>(this IEnumerable<T> list, string separator = ", ", string start = "[", string end = "]") {
            Debug.Log(list.ToListString());
        }

        public static string ToListString<T>(this IEnumerable<T> list, string separator = ", ", string start = "[", string end = "]") {
            return start + string.Join(separator, list.Select(item => item.ToString())) + end;
        }
        #endregion

        #region Sorting
        public static bool IsSorted<T>(this IList<T> list, Comparison<T> comparison) {
            for (var i = 0; i < list.Count-1; i++) {
                if (comparison(list[i], list[i + 1]) > 0) return false;
            }
            return true;
        }
        
        public static bool IsSorted<T>(this IList<T> list) where T : IComparable<T> {
            for (var i = 0; i < list.Count-1; i++) {
                if (list[i].CompareTo(list[i + 1]) > 0) return false;
            }
            return true;
        }
        
        public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison) {
            QuickSort_Impl(list, 0, list.Count-1, comparison);
        }

        public static void QuickSort<T>(this IList<T> list) where T : IComparable<T> {
            QuickSort_Impl(list, 0, list.Count-1);
        }

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
        
        public static void InsertionSort<T>(this IList<T> list) where T: IComparable<T> {
            var count = list.Count;
            for (var index1 = 1; index1 < count; ++index1) {
                var y = list[index1];
                int index2;
                for (index2 = index1 - 1; index2 >= 0 && list[index2].CompareTo(y) > 0; --index2)
                    list[index2 + 1] = list[index2];
                list[index2 + 1] = y;
            }
        }
        #endregion

        #region private Utilities
        private static void QuickSort_Impl<T>(this IList<T> list, int startIndex, int endIndex, Comparison<T> comparison) {
            if (startIndex >= endIndex)
                return;

            var partitionIndex = QuickSort_Partition(list, startIndex, endIndex, comparison);

            QuickSort_Impl(list, startIndex, partitionIndex - 1, comparison);
            QuickSort_Impl(list, partitionIndex + 1, endIndex, comparison);
        }

        private static void QuickSort_Impl<T>(this IList<T> list, int startIndex, int endIndex) where T : IComparable<T> {
            if (startIndex >= endIndex)
                return;

            var partitionIndex = QuickSort_Partition(list, startIndex, endIndex);

            QuickSort_Impl(list, startIndex, partitionIndex - 1);
            QuickSort_Impl(list, partitionIndex + 1, endIndex);
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

        private static int QuickSort_Partition<T>(IList<T> list, int low, int high) where T : IComparable<T> {
            var pivot = list[high];
            var lowIndex = low - 1;

            for (var j = low; j < high; j++)
                if (list[j].CompareTo(pivot) <= 0) {
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