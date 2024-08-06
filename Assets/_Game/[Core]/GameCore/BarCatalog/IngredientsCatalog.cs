using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.BarCatalog
{
	[CreateAssetMenu(fileName = "IngredientsCatalog", menuName = "Bar/IngredientsCatalog", order = 0)]
	public class IngredientsCatalog : ScriptableObject
	{
		[SerializeField] private List<BarIngredient> _barIngredients;

		public List<BarIngredient> IngredientsForType(DirectionType directionType)
			=> _barIngredients.Where(x => x.DirectionType == directionType).ToList();

		public void ResetSelected() => _barIngredients.ForEach(x => x.SetSelected(false));
	}
}