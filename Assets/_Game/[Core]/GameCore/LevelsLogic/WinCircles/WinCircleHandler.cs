using System;
using System.Collections.Generic;
using _Tools;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class WinCircleHandler : MonoBehaviour
	{
		public event Action OnWinLevel;

		[SerializeField] private List<WinCircle> _winCircles;

		public void StartRandomCircle()
		{
			_winCircles.ForEach(x => x.Deactivate());

			var randomCircle = _winCircles.GetRandomElement();
			randomCircle.Activate();
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