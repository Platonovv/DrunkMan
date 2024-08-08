using System;
using System.Collections.Generic;
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
		[SerializeField] protected CharacterAnimator _animator;
		[SerializeField] protected NavMeshAgent _agent;

		private CharacterData _characterData;
		private int _currentWaypointIndex;
		private bool _isMovingAgent;
		private float _health;

		protected bool IsMove;
		protected int CurrentPathIndex;
		protected readonly List<Vector3> WorldWaypoints = new();

		public void InitData(CharacterData characterData)
		{
			_characterData = characterData;
			_health = characterData.Health;

			_healthBar.SetMaxValue(_health, true);
			_healthBar.SetValue(_health);
			_agent.speed = characterData.Speed;
		}

		public void SetPosition(Transform pos)
		{
			ResetPath();
			transform.position = pos.position;
			_agent.enabled = true;
		}

		public void TakeDamage(float value)
		{
			_health = Mathf.Clamp(_health - value, 0, _characterData.Health);
			_healthBar.SetValue(_health);
			ResetPath();

			if (_health <= 0 && _health < _characterData.Health)
				OnDeath?.Invoke();
		}

		public void SetParent(Transform parent) => transform.SetParent(parent);

		public virtual void PlayAgent()
		{
		}

		public virtual void SetLineRenderer(List<Vector3> vector3S)
		{
		}

		public virtual void ClearLastPath(List<Vector3> vector3S)
		{
		}

		public virtual void MoveAgent()
		{
			IsMove = true;
			_currentWaypointIndex = default;
			SetNextWaypoint();
		}

		protected virtual void ResetPath()
		{
		}

		private void Update()
		{
			if (!IsMove)
				return;

			if (_agent.remainingDistance < 0.01f && _currentWaypointIndex < WorldWaypoints.Count)
				SetNextWaypoint();

			if (_agent.remainingDistance < 0.01f && !_agent.pathPending)
			{
				EndPath();
			}
		}

		protected virtual void EndPath() => OnEndPath?.Invoke();

		private void SetNextWaypoint()
		{
			if (_currentWaypointIndex >= WorldWaypoints.Count)
				return;

			var vector3 = WorldWaypoints[_currentWaypointIndex];
			_agent.SetDestination(vector3);
			_currentWaypointIndex++;
		}
	}
}