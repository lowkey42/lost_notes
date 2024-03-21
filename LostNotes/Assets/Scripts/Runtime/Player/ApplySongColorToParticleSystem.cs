using UnityEngine;

namespace LostNotes.Player {
	internal sealed class ApplySongColorToParticleSystem : MonoBehaviour, ISongMessages {
		[SerializeField]
		private ParticleSystem _particleSystem;

		public void OnSetSong(SongAsset song) {
			var main = _particleSystem.main;
			main.startColor = song.Color;
		}
	}
}
