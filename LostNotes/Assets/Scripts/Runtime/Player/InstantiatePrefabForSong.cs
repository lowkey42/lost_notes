using LostNotes.Level;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Player {
	internal sealed class InstantiatePrefabForSong : MonoBehaviour {
		[SerializeField, Expandable]
		private SongLoadout _songs;
		[SerializeField]
		private GameObject _prefab;

		private void OnEnable() {
			transform.Clear();

			foreach (var song in _songs.Songs) {
				var instance = Instantiate(_prefab, transform);
				instance.BroadcastMessage(nameof(ISongMessages.OnSetSong), song, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void OnDisable() {
			transform.Clear();
		}
	}
}
