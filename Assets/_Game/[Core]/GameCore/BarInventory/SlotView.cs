using _Game.BarCatalog;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.BarInventory
{
	public class SlotView : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Image _image;
		[SerializeField] private TMP_Text _price;
		[SerializeField] private TMP_Text _count;

		public void UpdateView(BarIngredient slotData)
		{
			_image.sprite = slotData.Icon;

			var slotDataPrice = slotData.Price > 0 ? slotData.Price.ToString() : "Free";
			_price.SetText($"{slotDataPrice}");
			UpdateCount(slotData.Count);
		}

		public void UpdateCount(int count) => _count.SetText($"{count}");
	}
}