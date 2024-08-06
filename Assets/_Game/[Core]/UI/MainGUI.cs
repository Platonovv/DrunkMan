using _Game.BarInventory;
using _Game.Mixer;
using UnityEngine;

namespace _Game.UI
{
	public class MainGUI : MonoBehaviour
	{
		[SerializeField] private Inventory _inventory;
		[SerializeField] private BaseMixerUI _baseMixerUI;

		public BaseMixerUI BaseMixerUI => _baseMixerUI;
		public Inventory Inventory => _inventory;
	}
}