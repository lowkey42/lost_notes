using System.Collections;
using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.UI {
	internal sealed class MoveIndicatorDrawer : MonoBehaviour, IActorMessages {
		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _moveChannelReference;
		private GameObjectEventChannel _moveChannel;

		private IEnumerator Start() {
			_ = transform.TryGetComponentInParent(out _actor);

			yield return _moveChannelReference.LoadAssetAsync(asset => {
				_moveChannel = asset;
				_moveChannel.onTrigger += HandleMove;
			});

			RecreateIndicators();
		}

		private void OnDestroy() {
			if (_moveChannel) {
				_moveChannel.onTrigger -= HandleMove;
				_moveChannelReference.ReleaseAsset();
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
