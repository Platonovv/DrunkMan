using System;
using System.Collections.Generic;
using System.Linq;
using _Game.DrunkManSpawner.Data;
using UI.ProgressBars;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Characters
{
	[SelectionBase]
	public class CharacterBase : MonoBehaviour
	{
		public event Action OnDeath;
		public event Action OnEndPath;

		[Header("Components")]
		[SerializeField] private ProgressBar _healthBar;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private LineRenderer _lineRenderer;
		[SerializeField] protected NavMeshAgent _agent;

		private DrunkManData _drunkManData;
		private int _currentWaypointIndex;

		private readonly List<Vector3> _worldWaypoints = new();

		private bool _isMove;
		private bool _isMovingAgent;
		private float _health;
		private List<Vector3> _path;
		private int _lastCount;

		public void InitData(DrunkManData drunkManData)
		{
			_drunkManData = drunkManData;
			_health = drunkManData.Health;

			_healthBar.SetMaxValue(_health, true);
			_healthBar.SetValue(_health);
			_agent.speed = drunkManData.Speed;
		}

		public void TakeDamage(float value)
		{
			_health = Mathf.Clamp(_health - value, 0, _drunkManData.Health);
			_healthBar.SetValue(_health);

			if (_health <= 0 && _health < _drunkManData.Health)
				OnDeath?.Invoke();
		}

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
					if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 0)
					{
						var pathCorner = path.corners.ToList();
						pathCorner.RemoveAt(0);
						pathPoints.AddRange(pathCorner);
					}
				}
				else if (NavMesh.SamplePosition(worldWaypoint, out var hitPos, 100.0f, NavMesh.AllAreas))
				{
					if (_agent.CalculatePath(hitPos.position, path))
					{
						if (path.corners.Length > 0)
						{
							var pathCorner = path.corners.ToList();
							pathCorner.RemoveAt(0);
							pathPoints.AddRange(pathCorner);
						}
					}
				}
			}

			_path = pathPoints;
			_lastCount = pathPoints.Count - _lastCount;
			_lineRenderer.positionCount = _path.Count;
			_lineRenderer.SetPositions(_path.ToArray());
		}

		public void MoveAgent()
		{
			_isMove = true;
			_currentWaypointIndex = default;
			SetNextWaypoint();
			_agent.isStopped = true;
			_animator.DoDrink(true);
		}

		public void ResetPath()
		{
			_path.Clear();
			_worldWaypoints.Clear();
			_lineRenderer.positionCount = default;

			_worldWaypoints.Add(transform.position);
		}

		public void ClearLastPath()
		{
			var indexDelete = _path.Count - _lastCount;

			if (indexDelete >= 0)
				_path.RemoveRange(indexDelete, _lastCount);

			_lineRenderer.positionCount -= _lastCount;
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

			if (_agent.remainingDistance < 0.01f && _currentWaypointIndex < _path.Count)
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
			if (_currentWaypointIndex >= _path.Count)
				return;

			var vector3 = _path[_currentWaypointIndex];
			_agent.SetDestination(vector3);
			_currentWaypointIndex++;
		}
	}
}