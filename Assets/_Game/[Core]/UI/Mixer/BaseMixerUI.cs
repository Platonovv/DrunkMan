using System;
using System.Collections.Generic;
using _Game.BarCatalog;
using _Tools;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Mixer
{
	public class BaseMixerUI : MonoBehaviour, IPointerMoveHandler, IDragSlotCollectionReceiver
	{
		public event Action OnStartSpawn;
		public event Action OnStartMix;
		public event Action OnStartMove;
		public event Action<BarIngredient> OnStartAgent;

		[SerializeField] private Button _startButton;
		[SerializeField] private Button _startMix;
		[SerializeField] private MixerTrigger _mixerTrigger;

		private readonly List<BarIngredient> _barIngredients = new();
		private bool _isClick;
		private bool _isStartMix;

		public void AddInCollection(BarIngredient slotData)
		{
			if (!_barIngredients.Contains(slotData))
				_barIngredients.Add(slotData);

			OnStartAgent?.Invoke(slotData);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			if (!_isStartMix)
				return;

			StartMove();
		}

		private void Awake()
		{
			_startButton.onClick.AddListener(StartLevel);
			_startMix.onClick.AddListener(StartMix);
			_mixerTrigger.OnAddCollection += AddInCollection;
		}

		private void OnDestroy()
		{
			_startButton.onClick.RemoveListener(StartLevel);
			_startMix.onClick.RemoveListener(StartMix);
			_mixerTrigger.OnAddCollection -= AddInCollection;
		}

		private void StartMix()
		{
			_isStartMix = true;
			OnStartMix?.Invoke();
		}

		public void EndMix() => _isStartMix = false;

		private void StartMove() => OnStartMove?.Invoke();

		private void StartLevel()
		{
			_startButton.Deactivate();
			OnStartSpawn?.Invoke();
		}
	}
}