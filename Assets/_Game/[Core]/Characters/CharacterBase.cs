using System;
using System.Collections.Generic;
using System.Linq;
using _Game.DrunkManSpawner.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Characters
{
	[SelectionBase]
	public class CharacterBase : MonoBehaviour
	{
		public event Action OnEndPath;

		[Header("Components")]
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private LineRenderer _lineRenderer;
		[SerializeField] protected NavMeshAgent _agent;

		private DrunkManData _drunkManData;
		private int _currentWaypointIndex;

		private readonly List<Vector3> _vector3S = new();
		private readonly List<Vector3> _worldWaypoints = new();

		private bool _isMove;
		private bool _isMovingAgent;
		public void InitData(DrunkManData drunkManData) => _drunkManData = drunkManData;

		public void SetPosition(Transform pos)
		{
			transform.position = pos.position;
			_agent.enabled = true;
			_worldWaypoints.Add(transform.position);
		}

		public void SetParent(Transform parent) => transform.SetParent(parent);

		public void PlayAgent()
		{
			/*_isMovingAgent = !_isMovingAgent;
			_agent.isStopped = _isMovingAgent;*/

			_agent.isStopped = false;
			_animator.DoMove(true);
		}

		public void SetLineRenderer(List<Vector3> vector3S)
		{
			SavePath(vector3S);
		}

		private void SavePath(List<Vector3> vector3S)
		{
			Vector3 worldWaypointCalculate = _worldWaypoints.Last();
			foreach (var vector3 in vector3S)
			{
				worldWaypointCalculate += vector3;
				_worldWaypoints.Add(worldWaypointCalculate);
			}

			List<Vector3> pathPoints = new List<Vector3>();

			foreach (Vector3 worldWaypoint in _worldWaypoints)
			{
				var path = new NavMeshPath();
				if (_agent.CalculatePath(worldWaypoint, path))
				{
					pathPoints.Add(path.corners.Last());
				}
			}

			_lineRenderer.positionCount = pathPoints.Count;
			_lineRenderer.SetPositions(pathPoints.ToArray());
			
			
		}

		public void MoveAgent(List<Vector3> vector3S)
		{
			_isMove = true;
			_currentWaypointIndex = default;
			_vector3S.AddRange(vector3S);
			SetNextWaypoint();
			_agent.isStopped = true;
			_animator.DoDrink(true);
		}

		public void ResetPath()
		{
			_worldWaypoints.Clear();
			_vector3S.Clear();
			_lineRenderer.positionCount = default;
			_worldWaypoints.Add(transform.position);
		}

		public void ClearLastPath(List<Vector3> vector3S)
		{
			var deleteCount = vector3S.Count;
			var indexDelete = _worldWaypoints.Count - deleteCount;
			
			if (indexDelete >= 0)
				_worldWaypoints.RemoveRange(indexDelete, deleteCount);

			_lineRenderer.positionCount -= vector3S.Count;
		}

		private void Awake()
		{
			_lineRenderer.startColor = Color.red;
			_lineRenderer.endColor = Color.red;
			_lineRenderer.startWidth = 0.1f;
			_lineRenderer.endWidth = 0.1f;
		}

		private void Update()
		{
			if (!_isMove)
				return;

			if (_agent.remainingDistance < 0.01f && _currentWaypointIndex < _vector3S.Count)
				SetNextWaypoint();

			if (_agent.remainingDistance < 0.01f && !_agent.pathPending)
			{
				_isMove = false;
				ResetPath();
				OnEndPath?.Invoke();
				_animator.DoMove(false);
				Debug.Log("End PAth");
			}
		}

		private void SetNextWaypoint()
		{
			if (_currentWaypointIndex >= _vector3S.Count)
				return;

			var vector3 = _vector3S[_currentWaypointIndex];
			var worldWaypoint = transform.position + vector3;
			_agent.SetDestination(worldWaypoint);
			_currentWaypointIndex++;
		}
	}
}