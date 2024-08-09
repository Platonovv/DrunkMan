namespace GameManager.LevelsLogic.Data
{
	public interface IQuestTarget
	{
		public EnemyType EnemyType { get; }
		public QuestComplete QuestComplete { get; }
		public void ActivateQuest(bool activate);
	}
}