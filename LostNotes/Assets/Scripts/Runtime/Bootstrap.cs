using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LostNotes {
	internal static class Bootstrap {
		private const string BOOTSTRAP_LABEL = "bootstrap";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Init() {
			var handle = Addressables.InstantiateAsync(BOOTSTRAP_LABEL);
			handle.Completed += OnComplete;
		}

		private static void OnComplete(AsyncOperationHandle<GameObject> handle) {
			Object.DontDestroyOnLoad(handle.Result);
		}
	}
}
