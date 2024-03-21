using System;
using System.Linq;
using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LostNotes.Level {
	[Serializable]
	internal sealed class SongLoadout {
		[SerializeField]
		private SerializableKeyValuePairs<SongAsset, bool> availability = new();

		internal void Load() {
			foreach (var (song, isAvailable) in availability) {
				song.IsAvailable = isAvailable;
			}
		}

		internal void Reset() {
			foreach (var (song, _) in availability) {
				song.IsAvailable = true;
			}
		}

#if UNITY_EDITOR
		internal bool EditorSetUp() {
			var songs = AssetDatabase
				.FindAssets($"t:{nameof(SongAsset)}")
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadMainAssetAtPath)
				.OfType<SongAsset>()
				.Where(s => !s.IsFailure);

			var isDirty = !songs.All(availability.ContainsKey);

			if (isDirty) {
				availability.SetItems(songs.ToDictionary(
					s => s,
					s => availability.TryGetValue(s, out var value) && value
				));
			}

			return isDirty;
		}
#endif
	}
}
