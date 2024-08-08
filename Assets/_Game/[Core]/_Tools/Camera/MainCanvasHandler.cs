using UnityEngine;
using UnityEngine.UI;

namespace _Tools
{
	public class MainCanvasHandler : MonoBehaviour
	{
		[SerializeField] private GraphicRaycaster _graphicRaycaster;

		public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
	}
}