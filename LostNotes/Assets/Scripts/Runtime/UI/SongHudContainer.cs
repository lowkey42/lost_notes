using System.Collections.Generic;
using LostNotes.Level;
using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.UI {
	internal sealed class SongHudContainer : MonoBehaviour {
		[SerializeField]
		private Transform _leftContainer;

		[SerializeField]
		private Transform _rightContainer;

		[SerializeField]
		private GameObject _prefab;

		[SerializeField, Expandable]
		private SongLoadout _songs;
		private readonly List<GameObject> _instances = new();

		private void OnEnable() {
			var i = 0;
			foreach (var song in _songs.Songs) {
				var parent = i % 2 == 0
					? _leftContainer
					: _rightContainer;
				var instance = Instantiate(_prefab, parent);
				instance.BroadcastMessage(nameof(ISongMessages.OnSetSong), song, SendMessageOptions.DontRequireReceiver);
				_instances.Add(instance);
				i++;
			}
		}

		private void OnDisable() {
			foreach (var obj in _instances) {
				if (obj) {
					Destroy(obj);
				}
			}

			_instances.Clear();
		}
	}
}
