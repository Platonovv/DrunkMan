using GameManager.LevelsLogic;
using UnityEngine;

namespace _Game._Core_.GameCoreEntry
{
	public class GameEntryPoint : MonoBehaviour
	{
		[SerializeField] private LevelPresenter _levelPresenter;
		[SerializeField] private GameLoop _gameLoop;

		private void Start()
		{
			_levelPresenter.OnLevelLoaded += InitLevelSystems;
			_levelPresenter.LoadLevel(0);
		}

		private void InitLevelSystems()
		{
			_levelPresenter.OnLevelLoaded -= InitLevelSystems;

			var level = FindObjectOfType<Level>();
			_gameLoop.Init(level);
		}
	}
}