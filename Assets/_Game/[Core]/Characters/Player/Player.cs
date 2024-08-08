using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Characters.Player
{
	public class Player : CharacterBase
	{
		[SerializeField] private LineRenderer _lineRenderer;

		private readonly Dictionary<int, List<Vector3>> _dictionaryPathCount = new();

		private void Awake()
		{
			_lineRenderer.startColor = Color.red;
			_lineRenderer.endColor = Color.red;
			_lineRenderer.startWidth = 0.1f;
			_lineRenderer.endWidth = 0.1f;
		}

		public override void PlayAgent()
		{
			_agent.isStopped = false;
			_animator.DoMove(true);
		}

		public override void SetLineRenderer(List<Vector3> vector3S)
		{
			SavePath(vector3S);
		}

		public override void ClearLastPath(List<Vector3> vector3S)
		{
			_lineRenderer.positionCount -= _dictionaryPathCount[CurrentPathIndex].Count;

			foreach (var vector3 in _dictionaryPathCount[CurrentPathIndex].ToList())
			{
				foreach (var worldWaypoint in WorldWaypoints.ToList().Where(worldWaypoint => worldWaypoint == vector3))
					WorldWaypoints.Remove(worldWaypoint);
			}

			_dictionaryPathCount.Remove(CurrentPathIndex);
		}

		public override void MoveAgent()
		{
			base.MoveAgent();
			_agent.isStopped = true;
			_animator.DoDrink(true);
			CurrentPathIndex++;
		}

		protected override void ResetPath()
		{
			WorldWaypoints.Clear();
			_dictionaryPathCount.Clear();
			CurrentPathIndex = default;
			_lineRenderer.positionCount = default;
			if (_agent.isOnNavMesh)
				_agent.ResetPath();
		}

		protected override void EndPath()
		{
			IsMove = false;
			ResetPath();
			_animator.DoMove(false);
			base.EndPath();
		}

		private void SavePath(List<Vector3> vector3S)
		{
			Vector3 worldWaypointCalculate;
			List<Vector3> currentVector3 = new List<Vector3>();

			switch (WorldWaypoints.Count)
			{
				case <= 0:
					worldWaypointCalculate = transform.position;
					WorldWaypoints.Add(worldWaypointCalculate);
					currentVector3.Add(worldWaypointCalculate);
					break;
				default:
					worldWaypointCalculate = WorldWaypoints.LastOrDefault();
					break;
			}

			foreach (var vector3 in vector3S)
			{
				worldWaypointCalculate += vector3;
				WorldWaypoints.Add(worldWaypointCalculate);
				currentVector3.Add(worldWaypointCalculate);
			}

			_lineRenderer.positionCount = WorldWaypoints.Count;
			_lineRenderer.SetPositions(WorldWaypoints.ToArray());
			_dictionaryPathCount.TryAdd(CurrentPathIndex, currentVector3);
		}
	}
}