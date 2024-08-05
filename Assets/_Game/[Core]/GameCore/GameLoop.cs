using System;
using _Game.Mixer;
using GameManager.LevelsLogic;
using Object = UnityEngine.Object;

namespace _Game
{
	public class GameLoop : IDisposable
	{
		private readonly LevelPresenter _levelPresenter;
		private readonly BaseMixerUI _currentMixer;
		private Level _currentLevel;

		public GameLoop(LevelPresenter levelPresenter, BaseMixerUI mixerUI)
		{
			_levelPresenter = levelPresenter;
			_currentMixer = mixerUI;
		}

		public void StartLevel()
		{
			_levelPresenter.OnLevelLoaded += InitLevelSystems;
			_levelPresenter.LoadLevel(default);
		}

		public void Dispose() => UnSubscribe();

		private void InitLevelSystems()
		{
			_levelPresenter.OnLevelLoaded -= InitLevelSystems;
			_currentLevel = Object.FindObjectOfType<Level>();

			_currentLevel.Init(_currentMixer);
			Subscribe();
		}

		private void Subscribe()
		{
			_currentLevel.OnLoseLevel += Lose;
		}

		private void UnSubscribe()
		{
			_currentLevel.OnLoseLevel -= Lose;
		}

		private void Win()
		{
		}

		private void Lose()
		{
			//окно луза
		}

		private void Restart()
		{
			//окно луза
		}
	}
}