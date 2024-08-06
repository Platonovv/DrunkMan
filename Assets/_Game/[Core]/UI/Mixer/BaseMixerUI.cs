using System;
using System.Collections.Generic;
using _Game.BarCatalog;
using _Tools;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Mixer
{
	public class BaseMixerUI : MonoBehaviour, IDragSlotCollectionReceiver
	{
		public event Action OnStartMixer;
		public event Action OnStartMove;
		public event Action<BarIngredient> OnDrawPath;

		[SerializeField] private Button _startButton;
		[SerializeField] private Button _startMove;
		[SerializeField] private MixerTrigger _mixerTrigger;

		private readonly List<BarIngredient> _barIngredients = new();

		public void AddInCollection(BarIngredient slotData)
		{
			if (!_barIngredients.Contains(slotData))
				_barIngredients.Add(slotData);
			
			OnDrawPath?.Invoke(slotData);
		}

		private void Awake()
		{
			_startButton.onClick.AddListener(StartLevel);
			_startMove.onClick.AddListener(StartMove);
			_mixerTrigger.OnAddCollection += AddInCollection;
		}

		private void OnDestroy()
		{
			_startButton.onClick.RemoveListener(StartLevel);
			_startMove.onClick.RemoveListener(StartMove);
			_mixerTrigger.OnAddCollection -= AddInCollection;
		}

		private void StartMove() => OnStartMove?.Invoke();

		private void StartLevel()
		{
			_startButton.Deactivate();
			OnStartMixer?.Invoke();
		}
	}
}