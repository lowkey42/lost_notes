using System;
using System.Collections.Generic;
using System.Linq;
using LostNotes.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace LostNotes.Level {
	[CreateAssetMenu]
	internal sealed class SongLoadout : ScriptableObject {
		[SerializeField, Expandable]
		private SongLoadout _allSongs;
		[SerializeField]
		private SongAsset[] _songs = Array.Empty<SongAsset>();
		public IEnumerable<SongAsset> Songs => _songs;

		internal void Load() {
			foreach (var song in _allSongs.Songs) {
				var isAvailable = _songs.Contains(song);
				song.IsAvailable = isAvailable;
				song.NotesLearned = isAvailable
					? song.NoteCount
					: 0;
			}
		}

		internal void Reset() {
			foreach (var song in _songs) {
				song.IsAvailable = true;
				song.NotesLearned = 0;
			}
		}
	}
}
