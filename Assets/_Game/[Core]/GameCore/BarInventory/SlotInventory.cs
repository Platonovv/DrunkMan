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
		private bool _activateSlot;
		public BarIngredient BarIngredient { get; set; }
		public Transform Transform { get; set; }
		public DraggedState DraggedState { get; set; }

		public void HideSlot() => ShowSlot(false);

		public void InitDragView(IngredientDraggedView ingredientDraggedView)
		{
			_ingredientDraggedView = ingredientDraggedView;
			Subscribe();
		}

		private void Subscribe()
		{
			Unsubscribe();
			_ingredientDraggedView.OnStartDrag += CheckState;
			_ingredientDraggedView.OnSelectedItem += SelectedItem;
			_ingredientDraggedView.OnReturnItem += ReturnItem;
			_ingredientDraggedView.OnEndDrag += CheckState;
		}

		public void InitSlot(BarIngredient slotData)
		{
			Transform = transform;
			BarIngredient = slotData;
			ShowSlot(true);
		}

		public void ReturnItem()
		{
			if (BarIngredient == default)
				return;

			if (_ingredientDraggedView.CurrentSlotData != BarIngredient)
				return;

			if (BarIngredient.Selected)
				return;

			BarIngredient.AddedCount(1);

			CheckTouch();
			UpdateSlot();
		}

		public void SelectedItem()
		{
			if (BarIngredient == default)
				return;

			if (_ingredientDraggedView.CurrentSlotData != BarIngredient)
				return;

			BarIngredient.SetSelected(true);
			CheckTouch();
			UpdateSlot();
		}

		private void Awake()
		{
			_slotView.OnBuyBottle += BuyBottle;
		}

		private void OnDestroy()
		{
			Unsubscribe();

			_slotView.OnBuyBottle -= BuyBottle;
		}

		private void Unsubscribe()
		{
			if (_ingredientDraggedView == default)
				return;

			_ingredientDraggedView.OnStartDrag -= CheckState;
			_ingredientDraggedView.OnSelectedItem -= SelectedItem;
			_ingredientDraggedView.OnReturnItem -= ReturnItem;
			_ingredientDraggedView.OnEndDrag -= CheckState;
		}

		private void BuyBottle()
		{
			if (BarIngredient == default)

				return;
			if (ResourceHandler.GetResourceCount(ResourceType.Money) < BarIngredient.Price)
				return;

			if (ResourceHandler.TrySubtractResource(ResourceType.Money, BarIngredient.Price))
				BarIngredient.AddedCount(1);
			
			UpdateSlot();
			CheckTouch();
		}

		private void ShowSlot(bool activate)
		{
			_activateSlot = activate;

			if (_activateSlot)
			{
				UpdateSlot();
				_slotCanvas.Show(_showDuration);
			}
			else
				_slotCanvas.Hide();

			CheckTouch();
		}

		private void UpdateSlot()
		{
			_slotView.UpdateView(BarIngredient);
			SetAlfa();
		}

		private void CheckTouch()
		{
			if (BarIngredient == default)
				return;

			if (_activateSlot
			    && BarIngredient.IngredientAvailable
			    && !BarIngredient.Selected)
			{
				_currentImage.raycastTarget = true;
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
				TakeItem();
		}

		private void TakeItem()
		{
			_currentImage.raycastTarget = false;
			BarIngredient.AddedCount(-1);
			UpdateSlot();
		}

		private void SetAlfa()
		{
			var color = _currentImage.color;
			color.a = BarIngredient.IngredientAvailable ? 1f : 0.5f;
			_currentImage.color = color;
		}
	}
}