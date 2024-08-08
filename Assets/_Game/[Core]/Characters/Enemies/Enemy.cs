namespace Gameplay.Characters.Enemies
{
	public class Enemy : CharacterBase
	{
		private bool _isMovingForward;

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