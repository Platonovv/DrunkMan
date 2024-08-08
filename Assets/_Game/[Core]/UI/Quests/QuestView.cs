using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI.Quests
{
	public class QuestView : MonoBehaviour
	{
		[SerializeField] private Image _questImage;

		public void SetQuestImage(Sprite sprite)
		{
			_questImage.sprite = sprite;
		}
	}
}