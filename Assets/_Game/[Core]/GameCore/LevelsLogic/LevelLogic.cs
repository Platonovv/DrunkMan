using System;
using _Tools.SceneReference;
using Gameplay.Characters;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	[Serializable]
	public class LevelLogic
	{
		[SerializeField] public CharacterBase PlayerPrefab;
		[SerializeField] private SceneWrapper _sceneWrapper;
		public SceneWrapper GetSelectedLevel() => _sceneWrapper;
	}
}