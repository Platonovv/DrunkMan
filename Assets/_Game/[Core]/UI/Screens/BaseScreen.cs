using System;
using _Tools;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI.Screens
{
	public class BaseScreen : MonoBehaviour
	{
		public event Action OnContinue;

		[Header("Components")]
		[SerializeField] protected CanvasGroup _canvasGroup;
		[SerializeField] private Button _continueButton;

		[Header("Settings")]
		[SerializeField] protected float _showAndHideDuration = 0.25f;

		public void Show() => _canvasGroup.Show(_showAndHideDuration);
		public void Hide() => _canvasGroup.Hide();

		protected virtual void Awake() => _continueButton.onClick.AddListener(OnClickContinue);

		protected virtual void OnDestroy() => _continueButton.onClick.RemoveListener(OnClickContinue);

		protected virtual void OnClickContinue() => OnContinue?.Invoke();
	}
}