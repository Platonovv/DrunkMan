using UnityEngine;
using UnityEngine.EventSystems;

namespace _Tools
{
	public class RawImageClickHandler : MonoBehaviour, IPointerClickHandler
	{
		public static bool IsClickedOnRawImage { get; private set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			IsClickedOnRawImage = true;
		}

		void LateUpdate()
		{
			IsClickedOnRawImage = false;
		}
	}
}