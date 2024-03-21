using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityObject = UnityEngine.Object;

namespace LostNotes {
	internal static class AssetReferenceExtensions {
		public static IEnumerator LoadAssetAsync<T>(this AssetReferenceT<T> assetReference, Action<T> callback) where T : UnityObject {
			var handle = assetReference.LoadAssetAsync();
			yield return handle;
			callback(handle.Result);
		}
	}
}
