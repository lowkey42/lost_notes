using System.Collections;

namespace LostNotes.Gameplay {
	public interface ITurnActor {

		IEnumerator DoTurn();

		bool HasTurnActions() { return true; }

	}
}
