using ScriptableObjects.Classes.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Characters
{
	[SelectionBase]
	public class CharacterBase : MonoBehaviour
	{
		[SerializeField] protected Rigidbody _rigidbody;
		[SerializeField] private CharacterAnimator _animator;

		[SerializeField] private ParticleSystem _stunParticle;
		[SerializeField] private Transform _hideResourcePoint;
		[SerializeField,] protected NavMeshAgent _agent;

		[SerializeField] protected CharacterData _characterData;

		public Transform HideResourcePoint => _hideResourcePoint;
		public CharacterAnimator CharAnimator => _animator;
		public CharacterData Data => _characterData;
		public NavMeshAgent Agent => _agent;
		protected ParticleSystem StunParticle => _stunParticle;
		protected bool IsArmed { get; set; }
	}
}