using System;
using _Game.BarCatalog;
using _Game.BarInventory;
using _Game.Mixer;
using _Game.UI;
using GameManager.LevelsLogic;
using Object = UnityEngine.Object;

namespace _Game
{
	public class GameLoop : IDisposable
	{
		private readonly LevelPresenter _levelPresenter;
		private readonly IngredientsCatalog _ingredientsCatalog;
		private readonly BaseMixerUI _currentMixer;
		private readonly Inventory _currentInventory;

		private Level _currentLevel;

		public GameLoop(LevelPresenter levelPresenter, MainGUI mainGUI)
		{
			_levelPresenter = levelPresenter;
			_currentMixer = mainGUI.BaseMixerUI;
			_currentInventory = mainGUI.Inventory;
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

			_currentLevel.Init(_currentMixer, _currentInventory);
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