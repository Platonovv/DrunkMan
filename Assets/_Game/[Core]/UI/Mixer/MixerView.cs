using System;
using _Game.BarCatalog;
using _Tools;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Mixer
{
	public class MixerView : MonoBehaviour, IDragSlotCollectionReceiver
	{
		public event Action OnClickStart;

		[SerializeField] private Button _startButton;

		private void Awake() => _startButton.onClick.AddListener(StartLevel);

		private void OnDestroy() => _startButton.onClick.RemoveListener(StartLevel);

		private void StartLevel()
		{
			_startButton.Deactivate();
			OnClickStart?.Invoke();
		}

		public void AddInCollection(BarIngredient slotData)
		{
		}
	}
}