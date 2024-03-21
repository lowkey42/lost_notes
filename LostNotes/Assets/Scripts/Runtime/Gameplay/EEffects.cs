using System;
using System.Collections.Generic;
using System.Linq;

namespace LostNotes.Gameplay {
	[Flags]
	internal enum EEffects {
		Noise = 1 << 0,
		Push = 1 << 1,
		Pull = 1 << 2,
		Sleep = 1 << 3,
		Calm = 1 << 4,
		Anger = 1 << 5,
		Attack = 1 << 6,
	}

	internal static class EEffectExtensions {
		private static readonly EEffects[] _effects = Enum
			.GetValues(typeof(EEffects))
			.Cast<EEffects>()
			.ToArray();

		public static IEnumerable<string> GetMessages(this EEffects effect) {
			for (var i = 0; i < _effects.Length; i++) {
				if (effect.HasFlag(_effects[i])) {
					yield return _effects[i] switch {
						EEffects.Pull => nameof(IEffectMessages.OnPull),
						EEffects.Push => nameof(IEffectMessages.OnPush),
						EEffects.Noise => nameof(IEffectMessages.OnNoise),
						EEffects.Sleep => nameof(IEffectMessages.OnSleep),
						EEffects.Calm => nameof(IEffectMessages.OnCalm),
						EEffects.Anger => nameof(IEffectMessages.OnAnger),
						EEffects.Attack => nameof(IEffectMessages.OnAttack),
						_ => throw new NotImplementedException(),
					};
				}
			}
		}
	}
}
