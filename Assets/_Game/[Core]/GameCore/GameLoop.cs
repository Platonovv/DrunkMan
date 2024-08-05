using GameManager.LevelsLogic;
using UnityEngine;

namespace _Game._Core_.GameCoreEntry
{
	public class GameLoop : MonoBehaviour
	{
		private Level _currentLevel;

		public void Init(Level level)
		{
			_currentLevel = level;
		}
		
		//Здесь вин,дуз, рестарт
	}
}