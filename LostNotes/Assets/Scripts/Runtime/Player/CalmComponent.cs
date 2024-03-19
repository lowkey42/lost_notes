using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class CalmComponent : MonoBehaviour {
		[SerializeField]
		private float radius = 5;

		private IEnumerator Start() {
			foreach (var position in GridUtils.GetCircle(Vector3Int.RoundToInt(transform.position), radius)) {
				var instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
				instance.transform.position = position;
				Destroy(instance, 1);
			}

			yield return Wait.forSeconds[1];
		}
	}
}
