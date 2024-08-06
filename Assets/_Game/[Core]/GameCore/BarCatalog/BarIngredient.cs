using System;
using UI;
using UnityEngine;

namespace _Game.BarCatalog
{
	[CreateAssetMenu(fileName = "BarIngredient", menuName = "Bar/Ingredient")]
	public class BarIngredient : ScriptableObject
	{
		[SerializeField] private Sprite _icon;
		[SerializeField] private DirectionType _directionType;
		[SerializeField] private ResourceType _resourceType;
		[SerializeField] private int _price;
		[SerializeField] private int _count;

		public Sprite Icon => _icon;
		public DirectionType DirectionType => _directionType;
		public int Price => _price;
		public int Count => _count;
		public ResourceType ResourceType => _resourceType;
	}
}