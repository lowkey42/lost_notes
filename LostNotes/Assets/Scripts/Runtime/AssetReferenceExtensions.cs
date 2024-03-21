using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityObject = UnityEngine.Object;

namespace LostNotes {
	internal static class AssetReferenceExtensions {
		public static IEnumerator LoadAssetAsync<T>(this AssetReferenceT<T> assetReference, Action<T> callback) where T : UnityObject {
			var handle = assetReference.LoadAssetAsync();

			yield return handle;

			if (handle.Status != AsyncOperationStatus.Succeeded) {
				Debug.LogError($"Failed to load asset reference '{assetReference}'!");
			}

			callback(handle.Result);
		}

		public static IEnumerator LoadAssetsAsync<T>(this string label, Action<T> callback) where T : UnityObject {
			var handle = Addressables.LoadResourceLocationsAsync(label);

			yield return handle;

			if (handle.Status != AsyncOperationStatus.Succeeded) {
				Debug.LogError($"Failed to load label '{label}'!");
			}

			yield return Addressables.LoadAssetsAsync(handle.Result, callback);
		}
	}
}
