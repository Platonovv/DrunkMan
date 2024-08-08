using System;
using Gameplay.Characters;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class WinCircle : MonoBehaviour
	{
		public event Action OnWinLevel;
		
		[SerializeField] private Sprite _spriteCircle;
		public Sprite SpriteCircle => _spriteCircle;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CharacterBase _))
				OnWinLevel?.Invoke();
		}
	}
}