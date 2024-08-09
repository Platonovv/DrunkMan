using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI.Quests
{
	public class QuestView : MonoBehaviour
	{
		[SerializeField] private Image _questImage;
		[SerializeField] private TMP_Text _rewardQuestText;

		public void SetQuestImage(Sprite sprite)
		{
			_questImage.sprite = sprite;
		}
		public void SetQuestReward(float value)
		{
			_rewardQuestText.SetText($"{value}<sprite=\"icon_gold\", index=0>");
		}
	}
}