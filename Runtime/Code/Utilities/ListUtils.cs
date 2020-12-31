using System.Collections.Generic;
using System.Linq;

namespace UnityCommons {
	public static class ListUtils {
		public static List<T> Combine<T>(params List<T>[] lists) {
			var combined = new List<T>();
			foreach (var list in lists) {
				combined.AddRange(list);
			}

			return combined;
		}
        
		public static List<T> Combine<T>(IEnumerable<IEnumerable<T>> lists) {
			return lists.SelectMany(list => list).ToList();
		}
		
		public static IEnumerable<T> Combine<T, TList>(IEnumerable<TList> lists) where TList : IEnumerable<T> {
			return lists.SelectMany(list => list);
		}
	}
}