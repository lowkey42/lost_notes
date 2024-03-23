using System;
using System.Collections;
using System.Collections.Generic;
using LostNotes.Level;
using LostNotes.Player;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LostNotes.Gameplay {
	internal sealed class AvatarActor : MonoBehaviour, ITurnActor, IAvatarMessages, IAttackMessages, IActorMessages {
		public static event Action<bool> OnChangeIsAlone;
		public static event Action<int> OnChangeAvailableActionPoints;
		public static event Action<int> OnChangeSelectedActionPoints;

		[Header("Components")]
		[SerializeField]
		private AvatarInput _input;
		[SerializeField]
		private LevelGridTransform _gridTransform;

		[SerializeField]
		private ActorStatus _status;

		public LevelComponent Level => _gridTransform.Level;

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

				OnChangeAvailableActionPoints?.Invoke(value);
			}
		}

		[Header("Song Playing")]
		[SerializeField]
		private TilemapMask _songRange = new(new Vector2Int(9, 9));

		public IEnumerable<Vector2Int> TilesInSongRange => _gridTransform
			.Level
			.TilesInArea(_gridTransform.Position2d, _gridTransform.Rotation2d, _songRange);

		public IEnumerable<GameObject> ObjectsInSongRange => _gridTransform
			.ObjectsInArea(_songRange);

		[Header("Runtime values")]
		[SerializeField, ReadOnly]
		private int _actionPoints = 0;

		private void Awake() {
			_input.CanMove = false;
			_input.CanChangePlayState = false;
		}

		private void OnEnable() {
			AvatarInput.OnChangePlayState += UpdatePlayState;
		}

		private void OnDisable() {
			AvatarInput.OnChangePlayState -= UpdatePlayState;
		}

		private void UpdatePlayState(EViolinState state) {
			OnChangeSelectedActionPoints?.Invoke(
				state switch {
					EViolinState.Playing => _actionPointsToPlay,
					_ => 0,
				}
			);
		}

		public TurnOrder TurnOrder { get; set; }

		private bool hasEnemies;

		public IEnumerator DoTurn() {
			if (!_status.HasStatusEffect(StatusEffects.Sleeping)) {
				ActionPoints = _actionPointsPerTurn;
				do {
					yield return _turnQueue.TryDequeue(out var coroutine)
						? coroutine
						: null;
				} while (_turnQueue.Count > 0 || _input.CanMove || _input.CanChangePlayState);
			}
		}

		[Header("Movement")]
		[SerializeField]
		private float _stepDurationFactor = 1.0f;

		[SerializeField]
		private float _stepJumpHeight = 0.2f;

		[SerializeField]
		private int _stepJumpCount = 3;

		[SerializeField]
		private float _sleepResetDelay = 2.0f;

		private readonly Queue<object> _turnQueue = new();

		public void OnMove(Vector2Int delta) {
			ActionPoints -= IsAlone
				? 0
				: _actionPointsToMove;

			_turnQueue.Enqueue(_gridTransform.MoveBy(delta, _stepJumpHeight, _stepDurationFactor, _stepJumpCount));
		}

		public void OnPlaySong(SongAsset song) {
			ActionPoints -= IsAlone
				? 0
				: _actionPointsToPlay;

			_turnQueue.Enqueue(song.PlaySong(_gridTransform, _songRange));
		}

		[ContextMenu(nameof(OnAttacked))]
		public void OnAttacked() {
			_status.ApplyStatusEffect(StatusEffects.Sleeping);
			ActionPoints = 0;
			StartCoroutine(ResetAfter(_sleepResetDelay));
		}

		private IEnumerator ResetAfter(float delay) {
			yield return new WaitForSeconds(delay);
			gameObject.BroadcastMessage(nameof(IAvatarMessages.OnReset), SendMessageOptions.DontRequireReceiver);
		}

		public void OnReset() {
			if (_deathChannel) {
				_deathChannel.Raise(gameObject);
			}
		}

		private bool _isAlone = false;

		private bool IsAlone {
			get => _isAlone;
			set {
				_isAlone = value;
				OnChangeIsAlone?.Invoke(value);
			}
		}

		public void OnStartTurn(TurnOrder round) {
			IsAlone = round.IsCurrentActorAlone;
		}

		public void OnEndTurn() {
		}

		public void OnPause() {
		}

		public void OnSkip() {
			ActionPoints = IsAlone
				? _actionPointsToPlay
				: 0;
		}
	}
}
