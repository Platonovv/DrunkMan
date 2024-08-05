using System.Collections.Generic;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	[CreateAssetMenu(menuName = "Configs/Scenes Container")]
	public class ScenesContainer : ScriptableObject
	{
		[SerializeField] private List<LevelLogic> _allScenes;

		private List<LevelLogic> _notNullScenes = new();
		public List<LevelLogic> Levels() => _allScenes;
	}
}