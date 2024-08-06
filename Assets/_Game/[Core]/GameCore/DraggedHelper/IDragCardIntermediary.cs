using _Game.BarCatalog;
using UnityEngine;

namespace UI.MainMenu
{
	public interface IDragCardIntermediary
	{
		public BarIngredient BarIngredient { get; set; }
		public Transform Transform { get; set; }
		public DraggedState DraggedState { get; set; }
		public void ReturnItem();
	}
}