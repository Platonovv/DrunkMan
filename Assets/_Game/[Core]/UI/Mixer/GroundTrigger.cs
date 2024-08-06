using UI.MainMenu.GangPage;
using UnityEngine;

namespace _Game.Mixer
{
	public class GroundTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out IngredientDraggedView ingredientDraggedView))
				ingredientDraggedView.ReturnItem();
		}
	}
}