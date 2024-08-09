using System;
using System.Collections.Generic;
using _Tools;
using GameManager.LevelsLogic.Data;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class QuestCircleHandler : MonoBehaviour
	{
		public event Action OnWinLevel;
		public event Action<QuestData> OnSpawnQuestCircle;
		public event Action<QuestData> OnSpawnAdditionalQuestCircle;

		[SerializeField] private List<QuestCircle> _winCircles;
		[SerializeField] private List<QuestCircle> _additionalQuestCircles;

		public void StartRandomQuest()
		{
			_winCircles.ForEach(x => x.Deactivate());

			var randomQuestCircle = _winCircles.GetRandomElement();
			randomQuestCircle.Activate();
			OnSpawnQuestCircle?.Invoke(randomQuestCircle.QuestData);
		}

		public void StartRandomAdditionalQuest()
		{
			_additionalQuestCircles.ForEach(x => x.Deactivate());

			var randomAdditionalQuestCircle = _additionalQuestCircles.GetRandomElement();
			randomAdditionalQuestCircle.Activate();
			OnSpawnAdditionalQuestCircle?.Invoke(randomAdditionalQuestCircle.QuestData);
		}

		private void Awake()
		{
			foreach (var winCircle in _winCircles)
				winCircle.OnWinLevel += WinLevel;
		}

		private void OnDestroy()
		{
			foreach (var winCircle in _winCircles)
				winCircle.OnWinLevel -= WinLevel;
		}

		private void WinLevel() => OnWinLevel?.Invoke();
	}
}