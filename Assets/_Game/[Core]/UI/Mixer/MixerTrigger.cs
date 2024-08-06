using System;
using _Game.BarCatalog;
using UI.MainMenu.GangPage;
using UnityEngine;

namespace _Game.Mixer
{
	public class MixerTrigger : MonoBehaviour
	{
		public event Action<BarIngredient> OnAddCollection;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out IngredientDraggedView ingredientDraggedView))
			{
				ingredientDraggedView.SelectedItem();
				OnAddCollection?.Invoke(ingredientDraggedView.CurrentSlotData);
			}
		}
	}
}