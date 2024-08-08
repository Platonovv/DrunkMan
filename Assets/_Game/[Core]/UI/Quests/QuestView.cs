using GameManager.LevelsLogic.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI.Quests
{
	public class QuestView : MonoBehaviour
	{
		[SerializeField] private Image _questImage;
		[SerializeField] private TMP_Text _rewardQuestText;

		public void SetQuest(Sprite sprite, QuestData questData)
		{
			_questImage.sprite = sprite;
			_rewardQuestText.SetText($"${questData.QuestReward}");
		}
	}
}