using System;
using System.Collections.Generic;
using _Game.BarCatalog;
using _Tools;
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
		[Header("Settings")]
		[SerializeField] private float _showDuration = 0.25f;

		public void Init()
		{
			SortInventory(DirectionType.Up);
			ShowInventory(true);
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

			foreach (var pageInventory in _pageInventories)
				pageInventory.OnClickPage += SortInventory;
		}

		private void OnDestroy()
		{
			foreach (var pageInventory in _pageInventories)
				pageInventory.OnClickPage -= SortInventory;
		}

		private void ShowInventory(bool active)
		{
			if (active)
				_canvasGroup.Show(_showDuration);
			else
				_canvasGroup.Hide();
		}
	}
}