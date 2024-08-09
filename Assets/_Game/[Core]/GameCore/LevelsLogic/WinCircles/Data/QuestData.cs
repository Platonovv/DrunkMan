using UnityEngine;

namespace GameManager.LevelsLogic.Data
{
	[CreateAssetMenu(fileName = "QuestData", menuName = "Quests/Quest", order = 0)]
	public class QuestData : ScriptableObject
	{
		[SerializeField] private Sprite _questSprite;
		[SerializeField] private int _questReward = 100;

		public Sprite QuestSprite => _questSprite;
		public int QuestReward => _questReward;
	}
}