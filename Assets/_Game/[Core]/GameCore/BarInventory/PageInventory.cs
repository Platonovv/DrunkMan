using System;
using _Game.BarCatalog;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.BarInventory
{
	public class PageInventory : MonoBehaviour
	{
		public event Action<DirectionType> OnClickPage;

		[Header("Components")]
		[SerializeField] private DirectionType _directionType;
		[SerializeField] private Button _pageButton;

		private void Awake() => _pageButton.onClick.AddListener(ClickPage);

		private void OnDestroy() => _pageButton.onClick.RemoveListener(ClickPage);

		private void ClickPage() => OnClickPage?.Invoke(_directionType);
	}
}