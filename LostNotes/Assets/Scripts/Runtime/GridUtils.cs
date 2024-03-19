using System.Collections.Generic;
using UnityEngine;

namespace LostNotes {
	internal static class GridUtils {
		public static IEnumerable<Vector3Int> GetCircle(Vector3Int position, float radius) {
			var extends = Vector3Int.CeilToInt(new(radius, 0, radius));
			var start = position - extends;
			var end = position + extends;

			for (var x = start.x; x <= end.x; x++) {
				for (var z = start.z; z <= end.z; z++) {
					var test = new Vector3Int(x, position.y, z);
					if (Vector3Int.Distance(position, test) < radius) {
						yield return test;
					}
				}
			}
		}
	}
}
