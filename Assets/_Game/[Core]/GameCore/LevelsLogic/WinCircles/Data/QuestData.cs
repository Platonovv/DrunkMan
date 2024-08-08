using UnityEngine;

namespace GameManager.LevelsLogic.Data
{
	[CreateAssetMenu(fileName = "QuestData", menuName = "Quests/Quest", order = 0)]
	public class QuestData : ScriptableObject
	{
		[SerializeField] private int _questReward = 100;

		public int QuestReward => _questReward;
	}
}