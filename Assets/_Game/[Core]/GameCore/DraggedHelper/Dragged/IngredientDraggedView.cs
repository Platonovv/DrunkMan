using System;
using _Game.BarCatalog;
using _Game.BarInventory;
using _Tools;
using UnityEngine;

namespace UI.MainMenu.GangPage
{
	public class IngredientDraggedView : MonoBehaviour
	{
		public event Action<bool> OnStartDrag;
		public event Action<bool> OnEndDrag;

		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private RectTransform _weaponCardMoveRect;
		[SerializeField] private SlotView _slotView;

		private RectTransform _transform;
		private BarIngredient _currentSlotData;
		private RectTransform CachedTransform => _transform ??= _weaponCardMoveRect;
		
		public SlotView SlotView => _slotView;
		public BarIngredient CurrentSlotData => _currentSlotData;

		public void SetDraggedView(BarIngredient slotData)
		{
			_currentSlotData = slotData;
			_slotView.UpdateView(slotData);
		}

		public void Move(Vector2 eventDataDelta) => _weaponCardMoveRect.anchoredPosition += eventDataDelta;

		public void StartDrag(Transform pos)
		{
			CachedTransform.position = pos.position;
			_canvasGroup.Show();
			OnStartDrag?.Invoke(true);
		}

		public void EndDrag()
		{
			_canvasGroup.Hide();
			OnEndDrag?.Invoke(false);
		}
	}
}