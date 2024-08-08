using System;
using System.Collections.Generic;
using _Tools;
using GameManager.LevelsLogic.Data;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class WinCircleHandler : MonoBehaviour
	{
		public event Action OnWinLevel;
		public event Action<Sprite, QuestData> OnSpawnWinCircle;

		[SerializeField] private List<WinCircle> _winCircles;

		public void StartRandomCircle()
		{
			_winCircles.ForEach(x => x.Deactivate());

			var randomCircle = _winCircles.GetRandomElement();
			randomCircle.Activate();

			OnSpawnWinCircle?.Invoke(randomCircle.SpriteCircle, randomCircle.QuestData);
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