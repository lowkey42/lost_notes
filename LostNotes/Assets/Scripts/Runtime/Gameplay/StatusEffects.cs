using System;

namespace LostNotes.Gameplay {
	[Flags]
	internal enum StatusEffects {
		Sleeping = 1 << 0,
		Angry = 1 << 1
	}
}
