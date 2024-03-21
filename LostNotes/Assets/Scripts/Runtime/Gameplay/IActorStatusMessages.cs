namespace LostNotes.Gameplay {
	internal interface IActorStatusMessages {
		void OnGainedStatusEffect(StatusEffects   gainedStatusEffect) { }
		void OnLostStatusEffect(StatusEffects     lostStatusEffect) { }
		void OnStatusEffectsChanged(StatusEffects statusEffects) { }
	}
}
