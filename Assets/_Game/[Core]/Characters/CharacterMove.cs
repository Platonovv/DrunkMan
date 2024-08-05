using UnityEngine;

namespace Gameplay.Characters
{
	public class CharacterMove : MonoBehaviour
	{
		[SerializeField,] private Animator _animator;
		[SerializeField,] private CharacterController _characterController;

		[SerializeField,] private float _moveSpeed = 5;
		[SerializeField,] private float _turnSmoothTime = 0.1f;
		[SerializeField,] private float _gravity = -9.81f;

		private float _turnSmoothVelocity;
		private Vector3 _velocity;
		private Transform _cameraTransform;
		private Transform _cachedTransform;

		private readonly int _speed = Animator.StringToHash("Speed");

		private void Awake()
		{
			_cachedTransform = transform;
			_cameraTransform = Camera.main.transform;
		}

		private void Update()
		{
			Move();
			Rotate();
			Animate();
		}

		private void Move()
		{
			_velocity.y += _gravity * Time.deltaTime;
			_velocity.y = Mathf.Clamp(_velocity.y, -5f, 0f);

			if (_characterController.isGrounded && _velocity.y < -2f)
				_velocity.y = -2f;

			/*if (TouchInput.Axis.sqrMagnitude > 0)
			{
				float targetAngle = Mathf.Atan2(TouchInput.Axis.x, TouchInput.Axis.y) * Mathf.Rad2Deg
				                    + _cameraTransform.eulerAngles.y;

				Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
				_characterController.Move(moveDir.normalized
				                          * TouchInput.Axis.magnitude
				                          * +_moveSpeed
				                          * Time.deltaTime);
				_characterController.Move(_velocity * Time.deltaTime);
			}*/
		}

		private void Rotate()
		{
			/*if (TouchInput.Axis.magnitude < 0.1)
				return;

			float targetAngle = Mathf.Atan2(TouchInput.Axis.x, TouchInput.Axis.y) * Mathf.Rad2Deg
			                    + _cameraTransform.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(_cachedTransform.eulerAngles.y,
			                                    targetAngle,
			                                    ref _turnSmoothVelocity,
			                                    _turnSmoothTime);
			_cachedTransform.rotation = Quaternion.Euler(0f, angle, 0f);*/
		}

		private void Animate()
		{
			//_animator.SetFloat(_speed, TouchInput.Axis.magnitude);
		}
	}
}