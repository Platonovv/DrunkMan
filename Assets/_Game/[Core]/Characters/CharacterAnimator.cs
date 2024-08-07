using UnityEngine;

namespace Gameplay.Characters
{
	public class CharacterAnimator : MonoBehaviour
	{
		[SerializeField] private Animator _animator;

		private static readonly int dead = Animator.StringToHash("Dead");
		private static readonly int drink = Animator.StringToHash("Drink");
		private static readonly int move = Animator.StringToHash("Move");

		public void DoDie(bool value) => _animator.SetBool(dead, value);
		public void DoDrink(bool value) => _animator.SetBool(drink, value);
		public void DoMove(bool value) => _animator.SetBool(move, value);
		public void EndDrink() => DoDrink(false);
	}
}