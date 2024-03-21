using LostNotes.Level;

namespace LostNotes.Gameplay {
	internal interface IEffectMessages {
		void OnNoise(LevelGridTransform source);
		void OnPush(LevelGridTransform source);
		void OnPull(LevelGridTransform source);
		void OnSleep(LevelGridTransform source);
		void OnCalm(LevelGridTransform source);
		void OnAnger(LevelGridTransform source);
		void OnAttack(LevelGridTransform source);
	}
}
