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
		private readonly Dictionary<int, List<Vector3>> _dictionaryPathCount = new();

		private bool _isMove;
		private bool _isMovingAgent;
		private float _health;
		private int _currentPathIndex;

		public void InitData(DrunkManData drunkManData)
		{
			_drunkManData = drunkManData;
			_health = drunkManData.Health;

			_healthBar.SetMaxValue(_health, true);
			_healthBar.SetValue(_health);
			_agent.speed = drunkManData.Speed;
		}

		public void SetPosition(Transform pos)
		{
			ResetPath();
			transform.position = pos.position;
			_agent.enabled = true;
		}

		public void TakeDamage(float value)
		{
			_health = Mathf.Clamp(_health - value, 0, _drunkManData.Health);
			_healthBar.SetValue(_health);
			ResetPath();

			if (_health <= 0 && _health < _drunkManData.Health)
				OnDeath?.Invoke();
		}

		public void SetParent(Transform parent) => transform.SetParent(parent);

		public void PlayAgent()
		{
			_agent.isStopped = false;
			_animator.DoMove(true);
		}

		public void SetLineRenderer(List<Vector3> vector3S)
		{
			SavePath(vector3S);
		}

		public void MoveAgent()
		{
			_isMove = true;
			_currentWaypointIndex = default;
			SetNextWaypoint();
			_agent.isStopped = true;
			_animator.DoDrink(true);
			_currentPathIndex++;
		}

		public void ResetPath()
		{
			_worldWaypoints.Clear();
			_dictionaryPathCount.Clear();
			_currentPathIndex = default;
			_lineRenderer.positionCount = default;
			if (_agent.isOnNavMesh)
				_agent.ResetPath();
		}

		public void ClearLastPath(List<Vector3> vector3S)
		{
			_lineRenderer.positionCount -= _dictionaryPathCount[_currentPathIndex].Count;

			foreach (var vector3 in _dictionaryPathCount[_currentPathIndex].ToList())
			{
				foreach (var worldWaypoint in _worldWaypoints.ToList().Where(worldWaypoint => worldWaypoint == vector3))
					_worldWaypoints.Remove(worldWaypoint);
			}

			_dictionaryPathCount.Remove(_currentPathIndex);
		}

		private void SavePath(List<Vector3> vector3S)
		{
			Vector3 worldWaypointCalculate;
			List<Vector3> currentVector3 = new List<Vector3>();

			switch (_worldWaypoints.Count)
			{
				case <= 0:
					worldWaypointCalculate = transform.position;
					_worldWaypoints.Add(worldWaypointCalculate);
					currentVector3.Add(worldWaypointCalculate);
					break;
				default:
					worldWaypointCalculate = _worldWaypoints.LastOrDefault();
					break;
			}

			foreach (var vector3 in vector3S)
			{
				worldWaypointCalculate += vector3;
				_worldWaypoints.Add(worldWaypointCalculate);
				currentVector3.Add(worldWaypointCalculate);
			}

			_lineRenderer.positionCount = _worldWaypoints.Count;
			_lineRenderer.SetPositions(_worldWaypoints.ToArray());
			_dictionaryPathCount.TryAdd(_currentPathIndex, currentVector3);
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

			if (_agent.remainingDistance < 0.01f && _currentWaypointIndex < _worldWaypoints.Count)
				SetNextWaypoint();

			if (_agent.remainingDistance < 0.01f && !_agent.pathPending)
			{
				_isMove = false;
				ResetPath();
				OnEndPath?.Invoke();
				_animator.DoMove(false);
			}
		}

		private void SetNextWaypoint()
		{
			if (_currentWaypointIndex >= _worldWaypoints.Count)
				return;

			var vector3 = _worldWaypoints[_currentWaypointIndex];
			_agent.SetDestination(vector3);
			_currentWaypointIndex++;
		}
	}
}