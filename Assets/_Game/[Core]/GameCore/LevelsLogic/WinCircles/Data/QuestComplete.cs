using System;
using Gameplay.Characters;
using UnityEngine;

namespace GameManager.LevelsLogic.Data
{
	public class QuestComplete : MonoBehaviour
	{
		public event Action OnTargetQuestComplete;

		private bool _activate;

		public void SetActivate(bool activate) => _activate = activate;

		private void OnTriggerEnter(Collider other)
		{
			if (!_activate)
				return;

			if (other.TryGetComponent(out CharacterBase _))
				OnTargetQuestComplete?.Invoke();
		}
	}
}