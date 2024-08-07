using System;
using _Game.BarInventory;
using _Game.Mixer;
using _Game.UI.Screens;
using UnityEngine;

namespace _Game.UI
{
	public class MainGUI : MonoBehaviour
	{
		public event Action<bool> OnContinueLevel;

		[SerializeField] private Inventory _inventory;
		[SerializeField] private BaseMixerUI _baseMixerUI;
		[SerializeField] private WinScreen _winScreen;
		[SerializeField] private LoseScreen _loseScreen;

		public BaseMixerUI BaseMixerUI => _baseMixerUI;
		public Inventory Inventory => _inventory;

		public void ShowWin() => _winScreen.Show();

		public void ShowLose() => _loseScreen.Show();

		public void HideScreens()
		{
			_winScreen.Hide();
			_loseScreen.Hide();
		}

		private void Awake()
		{
			_winScreen.OnContinue += ContinueLevel;
			_loseScreen.OnContinue += ResetLevel;
		}

		private void OnDestroy()
		{
			_winScreen.OnContinue -= ContinueLevel;
			_loseScreen.OnContinue -= ResetLevel;
		}

		private void ContinueLevel() => OnContinueLevel?.Invoke(true);
		private void ResetLevel() => OnContinueLevel?.Invoke(false);
	}
}