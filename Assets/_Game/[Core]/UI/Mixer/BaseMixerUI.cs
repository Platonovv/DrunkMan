using System;
using UnityEngine;

namespace _Game.Mixer
{
	public class BaseMixerUI : MonoBehaviour
	{
		public event Action OnStartMixer;

		[SerializeField] private MixerView _mixerView;

		private void Awake() => _mixerView.OnClickStart += StartLevel;

		private void OnDestroy() => _mixerView.OnClickStart -= StartLevel;

		private void StartLevel() => OnStartMixer?.Invoke();
	}
}