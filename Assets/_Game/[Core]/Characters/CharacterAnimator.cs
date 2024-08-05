using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Characters
{
	public class CharacterAnimator : MonoBehaviour
	{
		public event Action OnEquipWeapon;
		public event Action OnHit;
		public event Action OnPush;
		public event Action OnShacklesRemoved;
		public event Action OnFallingComplete;
		public event Action OnGettingUpComplete;

		[SerializeField] private Animator _animator;

		private static readonly int dead = Animator.StringToHash("Dead");
		private static readonly int escape = Animator.StringToHash("Escape");
		private static readonly int attack = Animator.StringToHash("Attack");
		private static readonly int onHand = Animator.StringToHash("OnHand");
		private static readonly int armed = Animator.StringToHash("Armed");
		private static readonly int target = Animator.StringToHash("Target");
		private static readonly int horizontal = Animator.StringToHash("Horizontal");
		private static readonly int vertical = Animator.StringToHash("Vertical");
		private static readonly int salute = Animator.StringToHash("Salute");
		private static readonly int idle = Animator.StringToHash("Idle");
		private static readonly int push = Animator.StringToHash("Push");
		private static readonly int boss = Animator.StringToHash("Boss");
		private static readonly int danceTrigger = Animator.StringToHash("Dance");
		private static readonly int sadTrigger = Animator.StringToHash("Sad");
		private static readonly int idleIndex = Animator.StringToHash("IdleIndex");
		private static readonly int idleCount = Animator.StringToHash("IdleCount");
		private static readonly int chainedIdleCount = Animator.StringToHash("ChainedIdleCount");
		private static readonly int attackSpeed = Animator.StringToHash("AttackSpeed");
		private static readonly int inShackles = Animator.StringToHash("InShackles");
		private static readonly int canRemoveShackles = Animator.StringToHash("CanRemoveShackles");

		private const string OneHanded = "OneHanded";

		public void DoDie(bool value) => _animator.SetBool(dead, value);
		public void DoEscape(bool value) => _animator.SetBool(escape, value);
		public void DoAttack(bool value) => _animator.SetBool(attack, value);
		public void DoOnHand(bool value) => _animator.SetBool(onHand, value);
		public void DoArmed(bool value) => _animator.SetBool(armed, value);
		public void DoTarget(bool value) => _animator.SetBool(target, value);
		public void DoSalute(bool value) => _animator.SetBool(salute, value);
		public void DoPush(bool value) => _animator.SetBool(push, value);
		public void DoBoss(bool value) => _animator.SetBool(boss, value);
		public void SetShackles(bool value) => _animator.SetBool(inShackles, value);
		public void SetCanRemoveShackles(bool value) => _animator.SetBool(canRemoveShackles, value);
		public void SetAttackSpeed(float speed) => _animator.SetFloat(attackSpeed, speed);
		public void DoStopIdle() => SetIdle(false);
		public void DoIdle() => DoIdle(_animator.GetInteger(idleCount));
		public void DoChainedIdle() => DoIdle(_animator.GetInteger(chainedIdleCount));

		public void SetOneHanded(bool value)
		{
			_animator.SetLayerWeight(_animator.GetLayerIndex(OneHanded), value ? 1 : 0);
		}

		public void DoDance() => _animator.SetTrigger(danceTrigger);
		public void DoSad() => _animator.SetTrigger(sadTrigger);

		private void SetIdle(bool value) => _animator.SetBool(idle, value);

		private void DoIdle(int index)
		{
			_animator.SetInteger(idleIndex, Random.Range(0, index));
			SetIdle(true);
		}

		private void SetTrigger(int id)
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != id)
				_animator.SetTrigger(id);
		}

		private void FallingComplete() => OnFallingComplete?.Invoke();

		private void GettingUpComplete() => OnGettingUpComplete?.Invoke();

		public void SetDirection(Vector2 dir)
		{
			_animator.SetFloat(horizontal, dir.x);
			_animator.SetFloat(vertical, dir.y);
		}

		public void Hit() => OnHit?.Invoke();

		public void Push() => OnPush?.Invoke();

		public void ShacklesRemoved() => OnShacklesRemoved?.Invoke();

		public void EquipWeapon()
		{
			OnEquipWeapon?.Invoke();
		}
	}
}