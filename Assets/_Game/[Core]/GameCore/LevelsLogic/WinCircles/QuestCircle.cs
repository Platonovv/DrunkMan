using System;
using GameManager.LevelsLogic.Data;
using Gameplay.Characters;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class QuestCircle : MonoBehaviour
	{
		public event Action OnWinLevel;

		[SerializeField] private QuestData _questData;
		public QuestData QuestData => _questData;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CharacterBase _))
				OnWinLevel?.Invoke();
		}
	}
}