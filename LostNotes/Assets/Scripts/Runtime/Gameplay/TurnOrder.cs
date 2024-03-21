using System.Collections.Generic;

namespace LostNotes.Gameplay {
	internal sealed class TurnOrder {
		public TurnOrder(List<ITurnActor> actors) {
			Actors = actors;
			CurrentActor = 0;
		}

		public List<ITurnActor> Actors { get; }
		public int CurrentActor { get; set; }

		public bool RoundDone => CurrentActor >= Actors.Count;
	}
}
