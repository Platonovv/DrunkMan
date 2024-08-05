using GameManager.LevelsLogic;
using UnityEngine;

namespace _Game
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LevelPresenter _levelPresenter;
		

		private void Start()
		{
			var gameLoop = new GameLoop(_levelPresenter);
			gameLoop.StartLevel();
		}
	}
}