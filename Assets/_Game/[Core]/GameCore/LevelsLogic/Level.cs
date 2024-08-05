using System;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class Level : MonoBehaviour
	{
		public event Action OnWinLevel;
		public event Action OnLoseLevel;
		
		
	}
}