using _Game.DrunkManSpawner.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Characters
{
	[SelectionBase]
	public class CharacterBase : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private ParticleSystem _stunParticle;
		[SerializeField] private Transform _hideResourcePoint;
		[SerializeField] protected NavMeshAgent _agent;
		
		private DrunkManData _drunkManData;

		public Transform HideResourcePoint => _hideResourcePoint;
		public CharacterAnimator CharAnimator => _animator;
		public DrunkManData Data => _drunkManData;
		public NavMeshAgent Agent => _agent;
		protected ParticleSystem StunParticle => _stunParticle;
		protected bool IsArmed { get; set; }
		
		public void InitData(DrunkManData drunkManData) => _drunkManData = drunkManData;
		
		public void SetPosition(Transform pos) => transform.position = pos.position;
	}
}