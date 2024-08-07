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
		public event Action OnSelectedItem;
		public event Action OnReturnItem;
		public event Action<bool> OnEndDrag;
		public event Action<BarIngredient> OnShowVisualPath;
		public event Action OnHideVisualPath;

		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private RectTransform _weaponCardMoveRect;
		[SerializeField] private SlotView _slotView;
		[SerializeField] private Rigidbody2D _rigidbody2D;

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
			_rigidbody2D.bodyType = RigidbodyType2D.Static;
			_rigidbody2D.transform.eulerAngles = Vector3.zero;
			_canvasGroup.Show();
			OnStartDrag?.Invoke(true);
			OnShowVisualPath?.Invoke(CurrentSlotData);
		}

		public void EndDrag()
		{
			_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
			OnEndDrag?.Invoke(false);
		}

		public void SelectedItem()
		{
			_rigidbody2D.bodyType = RigidbodyType2D.Static;
			_canvasGroup.Hide();
			OnSelectedItem?.Invoke();
		}

		public void ReturnItem()
		{
			_rigidbody2D.bodyType = RigidbodyType2D.Static;
			_canvasGroup.Hide();
			OnReturnItem?.Invoke();
			OnHideVisualPath?.Invoke();
		}
	}
}