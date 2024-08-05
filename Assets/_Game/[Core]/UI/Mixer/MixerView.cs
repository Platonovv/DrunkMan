using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Mixer
{
	public class MixerView : MonoBehaviour
	{
		public event Action OnClickStart;
		
		[SerializeField] private Button _startButton;

		private void Awake() => _startButton.onClick.AddListener(StartLevel);

		private void OnDestroy() => _startButton.onClick.RemoveListener(StartLevel);

		private void StartLevel() => OnClickStart?.Invoke();
	}
}