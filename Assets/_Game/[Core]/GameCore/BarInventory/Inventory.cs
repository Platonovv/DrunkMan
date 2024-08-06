using System.Collections.Generic;
using _Game.BarCatalog;
using _Tools;
using UI.MainMenu.GangPage;
using UI.MainMenu.GangPage.Dragged;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.BarInventory
{
	public class Inventory : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private List<PageInventory> _pageInventories;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private IngredientsCatalog _ingredientsCatalog;
		[SerializeField] private List<SlotInventory> _slotInventories;
		[Header("Handlers")]
		[SerializeField] private List<BarIngredientCollectionDragHandler> _collectionDragHandlers;
		[SerializeField] private IngredientDraggedView _slotDraggedView;

		[Header("Settings")]
		[SerializeField] private float _showDuration = 0.25f;

		public void Init()
		{
			_slotInventories.ForEach(x => x.InitDragView(_slotDraggedView));

			ResetSelectedSlots();
			SortInventory(DirectionType.Up);
		}

		public void ResetSelectedSlots()
		{
			_ingredientsCatalog.ResetSelected();
		}

		public void ShowInventory(bool active)
		{
			if (active)
				_canvasGroup.Show(_showDuration);
			else
				_canvasGroup.Hide();
		}

		private void SetNormalScroll()
		{
			_scrollRect.verticalNormalizedPosition = 1f;
		}

		private void SortInventory(DirectionType up)
		{
			_slotInventories.ForEach(x => x.HideSlot());
			var availableSlot = _ingredientsCatalog.IngredientsForType(up);

			for (var i = 0; i < availableSlot.Count; i++)
				_slotInventories[i].InitSlot(availableSlot[i]);

			SetNormalScroll();
		}

		private void Awake()
		{
			ShowInventory(false);
			_collectionDragHandlers.ForEach(x => x.InitWeaponCardDraggedView(_slotDraggedView));

			foreach (var pageInventory in _pageInventories)
				pageInventory.OnClickPage += SortInventory;
		}

		private void OnDestroy()
		{
			foreach (var pageInventory in _pageInventories)
				pageInventory.OnClickPage -= SortInventory;
		}
	}
}