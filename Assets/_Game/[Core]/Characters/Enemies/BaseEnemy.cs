using UnityEngine;

namespace Gameplay.Characters.Enemies
{
	public class BaseEnemy : MonoBehaviour
	{
		[SerializeField] private float _damage = 20f;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CharacterBase characterBase))
				characterBase.TakeDamage(_damage);
		}
	}
}