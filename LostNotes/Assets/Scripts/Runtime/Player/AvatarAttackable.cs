using LostNotes.Gameplay;
using UnityEngine;

namespace LostNotes.Player {
	public class AvatarAttackable : MonoBehaviour, IAttackMessages {
		public void OnAttacked() {
			Debug.Log("Player attacked"); // TODO: show game over message and restart level 
		}
	}
}
