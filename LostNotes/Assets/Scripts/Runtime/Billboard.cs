using UnityEngine;
using UnityEngine.Rendering;

namespace LostNotes {
	[ExecuteAlways]
	internal sealed class Billboard : MonoBehaviour {
		private void OnEnable() {
			RenderPipelineManager.beginCameraRendering += AlignCamera;
		}

		private void OnDisable() {
			RenderPipelineManager.beginCameraRendering -= AlignCamera;
		}

		private void AlignCamera(ScriptableRenderContext context, Camera camera) {
			transform.rotation = camera.transform.rotation;
		}
	}
}
