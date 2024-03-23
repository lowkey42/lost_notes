using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LostNotes.Gameplay;
using LostNotes.Level;
using LostNotes.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostNotes {
	internal class ActorAnimator : MonoBehaviour, IActorStatusMessages, INoteMessages, IEnemyMessages {
		private static readonly int _animatorIsWalking = Animator.StringToHash("IsWalking");
		private static readonly int _animatorIsActing = Animator.StringToHash("IsActing");
		private static readonly int _animatorIsSleeping = Animator.StringToHash("IsSleeping");
		private static readonly int _animatorWalkingSpeed = Animator.StringToHash("WalkingSpeed");
		private static readonly int _animatorActingSpeed = Animator.StringToHash("ActingSpeed");
		private static readonly int _animatorIdleSpeed = Animator.StringToHash("IdleSpeed");

		[SerializeField]
		[SerializedDictionary("Direction", "Animations")]
		private SerializedDictionary<LevelGridDirection, RuntimeAnimatorController> _animatorPerDirection;

		[SerializeField]
		private LevelGridTransform _levelGridTransform;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private float _walkingSpeed = 1.0f;

		[SerializeField]
		private float _actingSpeed = 1.0f;

		[SerializeField]
		private float _idleSpeed = 1.0f;

		private LevelGridDirection _lastDirection = LevelGridDirection.East;
		private RuntimeAnimatorController _defaultAnimationController;

		private TweenerCore<Vector3, Vector3, VectorOptions> _idleSequence;

		private bool _isWalking = false;
		private bool _isActing = false;
		private bool _isSleeping = false;

		private void OnValidate() {
			if (!_levelGridTransform)
				_levelGridTransform = GetComponentInParent<LevelGridTransform>();
			if (!_animator)
				_animator = GetComponentInParent<Animator>();
		}

		private void Start() {
			OnValidate();
			if (_animator) {
				_defaultAnimationController = _animator.runtimeAnimatorController;
				UpdateAnimator();
			}

			_idleSequence = transform.DOScaleY(0.95f, 0.333f).SetDelay(Random.Range(0f, 1f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce)
			                         .OnPause(() => transform.localScale = Vector3.one);
		}

		private void Update() {
			if (!_animator)
				return;
			
			_animator.SetBool(_animatorIsWalking, _isWalking = _levelGridTransform.IsMoving);
			_animator.SetFloat(_animatorWalkingSpeed, _walkingSpeed = 1.0f / _levelGridTransform.MovingDurationFactor);

			if (_levelGridTransform.Direction != _lastDirection)
				UpdateAnimator();
		}

		private void UpdateAnimator() {
			_lastDirection = _levelGridTransform.Direction;
			_animator.runtimeAnimatorController = _animatorPerDirection.GetValueOrDefault(_lastDirection, _defaultAnimationController);
			_animator.SetBool(_animatorIsWalking,  _isWalking);
			_animator.SetBool(_animatorIsActing,   _isActing);
			_animator.SetBool(_animatorIsSleeping, _isSleeping);
			_animator.SetFloat(_animatorWalkingSpeed, _walkingSpeed);
			_animator.SetFloat(_animatorActingSpeed,  _actingSpeed);
			_animator.SetFloat(_animatorIdleSpeed,    _idleSpeed);
		}

		public void OnGainedStatusEffect(StatusEffects gainedStatusEffect) {
			if (!_animator)
				return;

			if (gainedStatusEffect.HasFlag(StatusEffects.Sleeping)) {
				_animator.SetBool(_animatorIsSleeping, _isSleeping = true);
				_idleSequence.Pause();
			}
		}

		public void OnLostStatusEffect(StatusEffects lostStatusEffect) {
			if (!_animator)
				return;

			if (lostStatusEffect.HasFlag(StatusEffects.Sleeping)) {
				_animator.SetBool(_animatorIsSleeping, _isSleeping = false);
				_idleSequence.Play();
			}
		}

		public void OnStartPlaying() {
			if (!_animator)
				return;

			_animator.SetBool(_animatorIsActing, _isActing = true);
		}

		public void OnStopPlaying() {
			if (!_animator)
				return;

			_animator.SetBool(_animatorIsActing, _isActing = false);
		}

		public void OnStartNote(NoteAsset note) { }

		public void OnStopNote(NoteAsset note) { }

		public void OnStartAttack(float speed) {
			if (!_animator)
				return;

			_animator.SetFloat(_animatorActingSpeed, _actingSpeed = speed);
			_animator.SetBool(_animatorIsActing, _isActing = true);
		}

		public void OnEndAttack() {
			if (!_animator)
				return;

			_animator.SetBool(_animatorIsActing, _isActing = false);
		}
	}
}
