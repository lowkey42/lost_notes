using System.Collections;
using LostNotes.Level;
using LostNotes.Player;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Gameplay {
	internal sealed class AvatarActor : MonoBehaviour, ITurnActor, IAvatarMessages, IAttackMessages {
		[Header("Components")]
		[SerializeField]
		private AvatarInput _input;
		[SerializeField]
		private LevelGridTransform _gridTransform;

		[SerializeField]
		private AssetReferenceT<GameObjectEventChannel> _deathChannelReference;
		private GameObjectEventChannel _deathChannel;

		private IEnumerator Start() {
			yield return _deathChannelReference.LoadAssetAsync(asset => _deathChannel = asset);
		}

		private void OnDestroy() {
			if (_deathChannel) {
				_deathChannelReference.ReleaseAsset();
			}
		}

		[Header("Action Economy")]
		[SerializeField, Range(0, 10)]
		private int _actionPointsPerTurn = 3;
		[SerializeField]
		private bool _allowActionsWithoutSufficientPoints = true;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToMove = 1;
		[SerializeField, Range(0, 10)]
		private int _actionPointsToPlay = 2;
		private int ActionPoints {
			get => _actionPoints;
			set {
				_actionPoints = value;

				if (_allowActionsWithoutSufficientPoints) {
					_input.CanMove = _actionPoints > 0;
					_input.CanChangePlayState = _actionPoints > 0;
				} else {
					_input.CanMove = _actionPoints >= _actionPointsToMove;
					_input.CanChangePlayState = _actionPoints >= _actionPointsToPlay;
				}
			}
		}

		[Header("Song Playing")]
		[SerializeField]
		private TilemapMask _songRange = new(new Vector2Int(9, 9));

		[Header("Runtime values")]
		[SerializeField, ReadOnly]
		private int _actionPoints = 0;
		[SerializeField, ReadOnly]
		private bool _isAlive = true;

		private void Awake() {
			_input.CanMove = false;
			_input.CanChangePlayState = false;
		}

		public TurnOrder TurnOrder { get; set; }

		public IEnumerator DoTurn() {
			if (_isAlive) {
				ActionPoints = _actionPointsPerTurn;
				do {
					if (_songRoutine is { } coroutine) {
						_songRoutine = null;
						yield return coroutine;
					} else {
						yield return null;
					}
				} while (_input.CanMove || _input.CanChangePlayState);
			}
		}

		public void OnMove(Vector2Int delta) {
			ActionPoints -= _actionPointsToMove;
		}

		private IEnumerator _songRoutine;

		public void OnPlaySong(SongAsset song) {
			ActionPoints -= _actionPointsToPlay;

			_songRoutine = song.PlaySong(_gridTransform, _songRange);
		}

		[ContextMenu(nameof(OnAttacked))]
		public void OnAttacked() {
			_isAlive = false;
			ActionPoints = 0;

			gameObject.BroadcastMessage(nameof(IAvatarMessages.OnReset), SendMessageOptions.DontRequireReceiver);
		}

		public void OnReset() {
			if (_deathChannel) {
				_deathChannel.Raise(gameObject);
			}
		}
	}
}
