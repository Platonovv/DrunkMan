using System.Collections.Generic;
using UI;
using UnityEngine;

namespace _Game.BarCatalog
{
	[CreateAssetMenu(fileName = "BarIngredient", menuName = "Bar/Ingredient")]
	public class BarIngredient : ScriptableObject
	{
		[SerializeField] private Sprite _icon;
		[SerializeField] private Sprite _iconDragged;
		[SerializeField] private DirectionType _directionType;
		[SerializeField] private ResourceType _resourceType;
		[SerializeField] private int _price;
		[SerializeField] private int _count;
		
		[Header("WayPoints")]
		[SerializeField] private List<Vector3> _wayPoints;

		private bool _selected;
		public bool IngredientAvailable => CurrentCount > 0;
		public Sprite Icon => _icon;
		public Sprite IconDragged => _iconDragged;
		public DirectionType DirectionType => _directionType;
		public List<Vector3> WayPoints => _wayPoints;
		public int Price => _price;
		public int CurrentCount { get; set; }
		public ResourceType ResourceType => _resourceType;
		public bool Selected => _selected;
		public void SetSelected(bool selected) => _selected = selected;
		public void SetCurrentCount(int count) => CurrentCount = count;
		public void AddedCount(int value) => CurrentCount += value;
		private void OnEnable() => SetCurrentCount(_count);
		
	}
}