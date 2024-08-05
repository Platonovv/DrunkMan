using System;
using System.Collections;
using System.Collections.Generic;
using _Tools.SceneReference;
using Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager.LevelsLogic
{
	public class LevelPresenter : MonoBehaviour
	{
		public event Action OnLevelLoaded;

		[SerializeField] private ScenesContainer _scenesContainer;

		private static AsyncOperation AsyncOperationBasedOnCurrentLevelScene { get; set; }
		private SceneWrapper _currentSceneReference = null;
		private LevelLogic _currentLevelData;
		public IReadOnlyCollection<LevelLogic> AllLevels() => _scenesContainer.Levels();
		public LevelLogic CurrentLevelData => _currentLevelData;
		public CharacterBase PlayerView => _currentLevelData.PlayerPrefab;
		public int CurrentLevelId { get; private set; }

		public void LoadLevel(int id)

		{
			UnloadLevel();
			_currentLevelData = _scenesContainer.Levels()[id];
			_currentSceneReference = _currentLevelData.GetSelectedLevel();
			StartCoroutine(SceneLoadingBehavior());
			CurrentLevelId = id;
			AsyncOperationBasedOnCurrentLevelScene
				= SceneManager.LoadSceneAsync(_currentSceneReference.BuildIndex, LoadSceneMode.Additive);
			AsyncOperationBasedOnCurrentLevelScene.completed += InitializeLevelAfterSceneLoad;
		}

		private void InitializeLevelAfterSceneLoad(AsyncOperation _)
		{
			AsyncOperationBasedOnCurrentLevelScene.completed -= InitializeLevelAfterSceneLoad;
			OnLevelLoaded?.Invoke();
		}

		private IEnumerator SceneLoadingBehavior()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();

				if (AsyncOperationBasedOnCurrentLevelScene.isDone)
					break;
			}
		}

		public void UnloadLevel()
		{
			if (_currentSceneReference != null)
			{
				SceneManager.UnloadSceneAsync(_currentSceneReference.BuildIndex);
				_currentSceneReference = null;
			}
		}

	}
}