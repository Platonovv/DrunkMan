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
		private readonly MainGUI _mainGUI;

		private Level _currentLevel;

		public GameLoop(LevelPresenter levelPresenter, MainGUI mainGUI)
		{
			_levelPresenter = levelPresenter;
			_mainGUI = mainGUI;
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
			_currentLevel.OnWinLevel += Win;
			_currentLevel.OnLoseLevel += Lose;

			_mainGUI.OnContinueLevel += ContinueLogic;
		}

		private void UnSubscribe()
		{
			_currentLevel.OnWinLevel -= Win;
			_currentLevel.OnLoseLevel -= Lose;

			_mainGUI.OnContinueLevel -= ContinueLogic;
		}

		private void Win()
		{
			_mainGUI.HideInventory();
			_mainGUI.ShowWin();
		}

		private void Lose()
		{
			_mainGUI.HideInventory();
			_mainGUI.ShowLose();
		}

		private void ContinueLogic(bool continueLevel)
		{
			_mainGUI.HideScreens();

			if (continueLevel)
				_currentMixer.ContinueLevel();
			else
			{
				Object.Destroy(_currentLevel);
				_currentMixer.ResetLevel();
				StartLevel();
			}
		}
	}
}