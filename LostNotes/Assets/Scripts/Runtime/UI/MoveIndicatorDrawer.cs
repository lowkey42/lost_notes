using System.Collections;
using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.UI {
	internal sealed class MoveIndicatorDrawer : MonoBehaviour, IActorMessages {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> moveChannelReference;
		private GameObjectEventChannel moveChannel;

		private IEnumerator Start() {
			_ = transform.TryGetComponentInParent(out _actor);

			yield return moveChannelReference.LoadAssetAsync(asset => {
				moveChannel = asset;
				moveChannel.onTrigger += HandleMove;
			});

			RecreateIndicators();
		}

		private void OnDestroy() {
			if (moveChannel) {
				moveChannel.onTrigger -= HandleMove;
				moveChannelReference.ReleaseAsset();
			}
		}

		private ITurnActor _actor;

		private bool _ourTurn = false;

		private GameObject _turnIndicatorRoot;

		public void OnStartTurn(TurnOrder round) {
			_ourTurn = true;
		}

		public void OnEndTurn() {
			_ourTurn = false;
			RecreateIndicators();
		}

		private void HandleMove(GameObject obj) {
			RecreateIndicators();
		}

		private void ClearIndicators() {
			if (_turnIndicatorRoot)
				Destroy(_turnIndicatorRoot);
		}

		private void RecreateIndicators() {
			if (_actor == null || _ourTurn)
				return;

			ClearIndicators();
			_turnIndicatorRoot = new GameObject("Indicators");
			_actor.CreateTurnIndicators(_turnIndicatorRoot.transform);
		}
	}
}
