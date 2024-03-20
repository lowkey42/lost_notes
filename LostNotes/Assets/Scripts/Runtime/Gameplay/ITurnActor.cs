using System.Collections;
using UnityEngine;

namespace LostNotes.Gameplay {
	public interface ITurnActor {
#pragma warning disable IDE1006 // Naming Styles
		GameObject gameObject { get; }
#pragma warning restore IDE1006 // Naming Styles

		IEnumerator DoTurn();

		bool HasTurnActions() { return true; }
	}
}
