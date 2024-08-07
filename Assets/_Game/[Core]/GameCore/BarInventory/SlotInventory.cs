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

			if (BarIngredient.Price > 0)
				ResourceHandler.AddResource(ResourceType.Money, BarIngredient.Price);

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
			ResourceHandler.OnValueAdded += ValueChanged;
		}

		private void OnDestroy()
		{
			if (_ingredientDraggedView != default)
			{
				_ingredientDraggedView.OnStartDrag -= CheckState;
				_ingredientDraggedView.OnSelectedItem -= SelectedItem;
				_ingredientDraggedView.OnReturnItem -= ReturnItem;
				_ingredientDraggedView.OnEndDrag -= CheckState;
			}

			ResourceHandler.OnValueAdded -= ValueChanged;
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

		private void ValueChanged(ResourceType money, int value)
		{
			if (BarIngredient == default)
				return;

			CheckTouch();
		}

		private void CheckTouch()
		{
			if (BarIngredient == default)
				return;

			if (_activateSlot
			    && BarIngredient.IngredientAvailable
			    && ResourceHandler.GetResourceCount(ResourceType.Money) >= BarIngredient.Price
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

			if (BarIngredient.Price > 0)
				ResourceHandler.TrySubtractResource(ResourceType.Money, BarIngredient.Price);

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