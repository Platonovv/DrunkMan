using System;
using _Game.BarCatalog;
using _Tools;
using UI;
using UI.MainMenu;
using UI.MainMenu.GangPage;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.BarInventory
{
	public class SlotInventory : MonoBehaviour, IDragCardIntermediary
	{
		[Header("Components")]
		[SerializeField] private Image _currentImage;
		[SerializeField] private CanvasGroup _slotCanvas;
		[SerializeField] private SlotView _slotView;

		[Header("Settings")]
		[SerializeField] private float _showDuration = 0.25f;

		private IngredientDraggedView _ingredientDraggedView;
		private int _count;
		private bool _activateSlot;
		private bool IngredientAvailable => _count > 0;
		public BarIngredient BarIngredient { get; set; }
		public Transform Transform { get; set; }
		public DraggedState DraggedState { get; set; }

		public void HideSlot() => ShowSlot(false);

		public void InitDragView(IngredientDraggedView ingredientDraggedView)
		{
			_ingredientDraggedView = ingredientDraggedView;
			_ingredientDraggedView.OnStartDrag += CheckState;
			_ingredientDraggedView.OnEndDrag += CheckState;
		}

		public void InitSlot(BarIngredient slotData)
		{
			Transform = transform;
			UpdateSlot(slotData);
			ShowSlot(true);
		}

		private void Awake()
		{
			ResourceHandler.OnValueAdded += ValueChanged;
		}

		private void OnDestroy()
		{
			_ingredientDraggedView.OnStartDrag -= CheckState;
			_ingredientDraggedView.OnEndDrag -= CheckState;

			ResourceHandler.OnValueAdded -= ValueChanged;
		}

		private void ShowSlot(bool activate)
		{
			_activateSlot = activate;

			if (_activateSlot)
				_slotCanvas.Show(_showDuration);
			else
				_slotCanvas.Hide();

			CheckTouch(_activateSlot);
		}

		private void UpdateSlot(BarIngredient slotData)
		{
			BarIngredient = slotData;
			_count = slotData.Count;
			_slotView.UpdateView(slotData);
		}

		private void ValueChanged(ResourceType money, int value)
		{
			if (BarIngredient == default)
				return;

			CheckTouch(_activateSlot);
			SetAlfa();
		}

		private void CheckTouch(bool activate)
		{
			if (BarIngredient == default)
				return;
			
			if (activate && IngredientAvailable && ResourceHandler.GetResourceCount(ResourceType.Money) >= BarIngredient.Price)
			{
				DraggedState = DraggedState.CanTouch;
			}
			else
				DraggedState = DraggedState.CantTouch;
			
		}

		private void CheckState(bool startDrag)
		{
			if (_ingredientDraggedView.CurrentSlotData != BarIngredient)
				return;

			if (startDrag)
				SetSelected();
			else
				SetDeSelected();

			_slotView.UpdateCount(_count);

			SetAlfa();
		}

		private void SetAlfa()
		{
			var color = _currentImage.color;
			color.a = IngredientAvailable ? 1f : 0.5f;
			_currentImage.color = color;
		}

		private void SetSelected()
		{
			_count -= 1;
			_count = Mathf.Clamp(_count, 0, int.MaxValue);

			if (BarIngredient.Price > 0)
				ResourceHandler.TrySubtractResource(ResourceType.Money, BarIngredient.Price);
		}

		private void SetDeSelected()
		{
			_count += 1;
			_count = Mathf.Clamp(_count, 0, int.MaxValue);

			if (BarIngredient.Price > 0)
				ResourceHandler.AddResource(ResourceType.Money, BarIngredient.Price);
		}
	}
}