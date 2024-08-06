using _Game.UI;
using GameManager.LevelsLogic;
using UnityEngine;

namespace _Game
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LevelPresenter _levelPresenter;
		[SerializeField] private MainGUI _mainGUI;
		

		private void Start()
		{
			var gameLoop = new GameLoop(_levelPresenter, _mainGUI);
			gameLoop.StartLevel();
		}
	}
}