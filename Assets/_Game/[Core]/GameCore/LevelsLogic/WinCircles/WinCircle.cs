using System;
using Gameplay.Characters;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class WinCircle : MonoBehaviour
	{
		public event Action OnWinLevel;
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CharacterBase characterBase))
				OnWinLevel?.Invoke();
		}
	}
}