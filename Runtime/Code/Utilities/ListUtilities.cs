using System.Collections.Generic;
using System.Linq;

namespace UnityCommons {
	public static class ListUtilities {
		/// <summary>
		/// Combines <paramref name="lists"/> into a single list
		/// </summary>
		public static List<T> Combine<T>(params List<T>[] lists) {
			List<T> combined = new List<T>();
			foreach (List<T> list in lists) {
				combined.AddRange(list);
			}

			return combined;
		}
		
		/// <summary>
		/// Combines <paramref name="lists"/> into a single list
		/// </summary>
		public static List<T> Combine<T>(IEnumerable<IEnumerable<T>> lists) {
			return lists.SelectMany(list => list).ToList();
		}
		
		/// <summary>
		/// Combines <paramref name="lists"/> into a single list
		/// </summary>
		public static IEnumerable<T> Combine<T, TList>(IEnumerable<TList> lists) where TList : IEnumerable<T> {
			return lists.SelectMany(list => list);
		}
	}
}