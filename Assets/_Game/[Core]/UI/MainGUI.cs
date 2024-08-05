using _Game.Mixer;
using UnityEngine;

namespace _Game.UI
{
	public class MainGUI : MonoBehaviour
	{
		[SerializeField] private BaseMixerUI _baseMixerUI;

		public BaseMixerUI BaseMixerUI => _baseMixerUI;
	}
}