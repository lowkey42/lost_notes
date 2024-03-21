namespace LostNotes.Gameplay {
	internal interface IActorMessages {
		void OnStartTurn(TurnOrder round);
		void OnEndTurn();
		void OnStartAnyTurn(TurnOrder round) { }
	}
}
