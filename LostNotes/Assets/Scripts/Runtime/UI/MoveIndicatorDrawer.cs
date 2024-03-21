using System.Collections;
using LostNotes.Gameplay;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.UI {
	internal sealed class MoveIndicatorDrawer : MonoBehaviour, IActorMessages {
		[SerializeField]
		private float _minAlpha = 0.2f;

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _moveChannelReference;
		private GameObjectEventChannel _moveChannel;

		private IEnumerator Start() {
			_ = transform.TryGetComponentInParent(out _actor);

			yield return _moveChannelReference.LoadAssetAsync(asset => {
				_moveChannel = asset;
				_moveChannel.OnTrigger += HandleMove;
			});

			RecreateIndicators();
		}

		private void OnDisable() {
			ClearIndicators();
		}

		private void OnDestroy() {
			if (_moveChannel) {
				_moveChannel.OnTrigger -= HandleMove;
				_moveChannelReference.ReleaseAsset();
			}
		}

		private ITurnActor _actor;

		private bool _ourTurn = false;

		private GameObject _turnIndicatorRoot;

		public void OnStartTurn(TurnOrder round) {
			_ourTurn = true;
		}

		public void OnStartAnyTurn(TurnOrder round) {
			RecreateIndicators();
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

			var distance = _actor.TurnOrder?.GetTurnOrderDistance(_actor) ?? 0;
			if (distance >= 1 && _actor.TurnOrder != null) {
				foreach (var sprite in _turnIndicatorRoot.GetComponentsInChildren<SpriteRenderer>()) {
					var alpha = 1.0f - ((distance - 1) / (float) (_actor.TurnOrder.Actors.Count - 1));
					sprite.color = sprite.color.WithAlpha((alpha * (1 - _minAlpha)) + _minAlpha);
				}
			}
		}
	}
}
