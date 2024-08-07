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
		private readonly List<Vector3> _path = new();
		private readonly Dictionary<int, int> _dictionaryPathCount = new();

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

		public void TakeDamage(float value)
		{
			_health = Mathf.Clamp(_health - value, 0, _drunkManData.Health);
			_healthBar.SetValue(_health);

			if (_health <= 0 && _health < _drunkManData.Health)
				OnDeath?.Invoke();
		}

		public void SetPosition(Transform pos)
		{
			ResetPath();
			transform.position = pos.position;
			_agent.enabled = true;
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
			_path.Clear();
			_worldWaypoints.Clear();
			_dictionaryPathCount.Clear();
			_currentPathIndex = default;
			_lineRenderer.positionCount = default;
		}

		public void ClearLastPath(List<Vector3> vector3S)
		{
			var lineRendererPositionCount = _dictionaryPathCount[_currentPathIndex];
			if (_currentPathIndex > 0)
			{
				lineRendererPositionCount = _dictionaryPathCount[_currentPathIndex]
				                            - _dictionaryPathCount[_currentPathIndex - 1];
			}

			_path.RemoveRange(_path.Count - lineRendererPositionCount, lineRendererPositionCount);
			_lineRenderer.positionCount -= lineRendererPositionCount;
			_dictionaryPathCount.Remove(_currentPathIndex);
			
			
			var deleteCount = vector3S.Count;
			var indexDelete = _worldWaypoints.Count - deleteCount;
			
			if (indexDelete >= 0)
				_worldWaypoints.RemoveRange(indexDelete, deleteCount);
		}

		private void SavePath(List<Vector3> vector3S)
		{
			Vector3 worldWaypointCalculate = _worldWaypoints.LastOrDefault();

			if (worldWaypointCalculate == default)
				worldWaypointCalculate = transform.TransformPoint(transform.localPosition);

			foreach (var vector3 in vector3S)
			{
				worldWaypointCalculate += vector3;
				_worldWaypoints.Add(worldWaypointCalculate);
			}

			foreach (var worldWaypoint in _worldWaypoints)
			{
				var findWaypoint = worldWaypoint;
				if (NavMesh.SamplePosition(worldWaypoint, out var hitPos, 100.0f, NavMesh.AllAreas))
					findWaypoint = hitPos.position;

				var path = new NavMeshPath();
				var pathGenerate = NavMesh.CalculatePath(transform.position, findWaypoint, NavMesh.AllAreas, path);

				if (!pathGenerate)
					return;

				foreach (var pathCorner in path.corners)
				{
					if (!_path.Contains(pathCorner))
						_path.Add(pathCorner);
				}
			}

			_lineRenderer.positionCount = _path.Count;
			_lineRenderer.SetPositions(_path.ToArray());
			_dictionaryPathCount.TryAdd(_currentPathIndex, _path.Count);
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