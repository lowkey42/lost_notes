using System.Collections.Generic;

namespace LostNotes.Gameplay {
	internal sealed class TurnOrder {
		public TurnOrder(IReadOnlyList<ITurnActor> actors) {
			Actors = actors;
			CurrentActor = 0;
		}

		public IReadOnlyList<ITurnActor> Actors { get; }
		public int CurrentActor { get; set; }

		public bool RoundDone => CurrentActor >= Actors.Count;
	}
}
