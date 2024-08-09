using _Game.DrunkManSpawner.Data;
using GameManager.LevelsLogic.Data;
using UnityEngine;

namespace Gameplay.Characters.Enemies
{
	public class Enemy : CharacterBase, IQuestTarget
	{
		private bool _isMovingForward;

		[SerializeField] private QuestComplete _questComplete;

		private bool _activateQuest;
		private EnemyData _enemyData;
		public EnemyType EnemyType => _enemyData.EnemyType;
		public QuestComplete QuestComplete => _questComplete;
		public void ActivateQuest(bool activate) => _questComplete.SetActivate(activate);

		public override void InitData(CharacterData characterData)
		{
			base.InitData(characterData);

			if (characterData is EnemyData enemyData)
				_enemyData = enemyData;
		}

		public override void MoveAgent()
		{
			base.MoveAgent();
			_animator.DoMove(true);
		}

		protected override void SetNextWaypoint()
		{
			var vector3 = WorldWaypoints[CurrentWaypointIndex];
			_agent.SetDestination(vector3);

			if (_isMovingForward)
			{
				CurrentWaypointIndex++;

				if (CurrentWaypointIndex < WorldWaypoints.Count)
					return;

				CurrentWaypointIndex = WorldWaypoints.Count - 2;
				_isMovingForward = false;
			}
			else
			{
				CurrentWaypointIndex--;

				if (CurrentWaypointIndex >= 0)
					return;

				CurrentWaypointIndex = 1;
				_isMovingForward = true;
			}
		}
	}
}