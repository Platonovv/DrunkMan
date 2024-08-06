using _Game.BarCatalog;
using _Tools;
using UnityEngine;

namespace _Game.BarInventory
{
	public class SlotInventory : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private CanvasGroup _slotCanvas;
		[SerializeField] private SlotView _slotView;

		[Header("Settings")]
		[SerializeField] private float _showDuration = 0.25f;

		private BarIngredient _currentSlotData;

		public void HideSlot()
		{
			ShowSlot(false);
		}

		private void ShowSlot(bool activate)
		{
			if (activate)
				_slotCanvas.Show(_showDuration);
			else
				_slotCanvas.Hide();
		}

		public void InitSlot(BarIngredient slotData)
		{
			UpdateSlot(slotData);
			ShowSlot(true);
		}

		private void UpdateSlot(BarIngredient slotData)
		{
			_currentSlotData = slotData;
			_slotView.UpdateView(slotData);
		}
	}
}