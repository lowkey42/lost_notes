using System.Linq;
using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LostNotes.Level {
	[CreateAssetMenu]
	internal sealed class SongLoadout : ScriptableObject {
		[SerializeField]
		private SerializableKeyValuePairs<SongAsset, bool> _availability = new();

		public IEnumerable<SongAsset> Songs => _availability.Keys;

		internal void Load() {
			foreach (var (song, isAvailable) in _availability) {
				song.IsAvailable = isAvailable;
			}
		}

		internal void Reset() {
			foreach (var (song, _) in _availability) {
				song.IsAvailable = true;
			}
		}

#if UNITY_EDITOR
		private void OnValidate() {
			var songs = AssetDatabase
				.FindAssets($"t:{nameof(SongAsset)}")
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadMainAssetAtPath)
				.OfType<SongAsset>()
				.Where(s => !s.IsFailure);

			var isDirty = !songs.All(_availability.ContainsKey);

			if (isDirty) {
				_availability.SetItems(songs.ToDictionary(
					s => s,
					s => _availability.TryGetValue(s, out var value) && value
				));
			}

			if (isDirty) {
				EditorUtility.SetDirty(this);
			}
		}
#endif
	}
}
