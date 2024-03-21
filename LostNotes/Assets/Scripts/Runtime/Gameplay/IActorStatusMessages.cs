namespace LostNotes.Gameplay {
	internal interface IActorStatusMessages {
		void OnGainedStatusEffect(StatusEffects statusEffect);
		void OnLostStatusEffect(StatusEffects   statusEffect);
	}
}
