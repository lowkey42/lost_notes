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

		public int GetTurnOrderDistance(ITurnActor actor) {
			var index = Actors.IndexOf(actor);
			if (index == -1)
				return -1;

			var turnDistance = 0;
			for (var i = CurrentActor % Actors.Count; i != index; i = (i + 1) % Actors.Count) {
				if (Actors[i].HasTurnActions())
					turnDistance++;
			}

			return turnDistance;
		}
	}
}
