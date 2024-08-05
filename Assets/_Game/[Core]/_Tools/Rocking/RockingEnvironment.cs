using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rocking
{
	public class RockingEnvironment : MonoBehaviour
	{
		[Header("Component")]
		[SerializeField] private Transform _model;

		[Header("Move")]
		[SerializeField] private bool _isNeedMove;
		[SerializeField] private Vector3 _moveVector;
		[SerializeField] private float _moveDuration;

		[Header("Rotate")]
		[SerializeField] private bool _isNeedRotate;
		[SerializeField] private bool _isRestartRotate;
		[SerializeField] private Vector3 _rotateVector;
		[SerializeField] private float _rotateDuration;

		[Header("Shake")]
		[SerializeField] private bool _isNeedShake;
		[SerializeField] private bool _isRandomDelay;
		[SerializeField] private bool _positionShake;
		[SerializeField] private bool _rotationShake;
		[SerializeField] private float _delay = 1.5f;
		[SerializeField] private Vector2 _randomVector;
		[SerializeField] private float _shakeDuration = 0.5f;
		[SerializeField] private float _strength = 10;
		[SerializeField] private int _vibrato = 50;
		private Coroutine _shakeCoroutine;

		private void OnEnable()
		{
			StartAnimation();
			
			if (_isNeedShake)
				Shake();
		}

		private void StartAnimation()
		{
			_model.DOKill();
			
			if (_isNeedMove)
				Move();
			if (_isNeedRotate)
				Rotate();
			
		}
		
		private void Move()
		{
			_model.DOLocalMove(_moveVector, _moveDuration)
				  .SetLink(_model.transform.gameObject)
				  .SetEase(Ease.Linear)
				  .SetLoops(-1, _isRestartRotate ? LoopType.Restart : LoopType.Yoyo);
		}
		
		private void Rotate()
		{
			_model.DOLocalRotate(_rotateVector, _rotateDuration, RotateMode.LocalAxisAdd)
				  .SetLink(_model.gameObject)
				  .SetEase(Ease.Linear)
				  .SetLoops(-1, _isRestartRotate ? LoopType.Restart : LoopType.Yoyo);
		}

		private void Shake()
		{
			//_shakeCoroutine.Stop(this);
			_shakeCoroutine = StartCoroutine(ShakeCoroutine());
		}

		private IEnumerator ShakeCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(_isRandomDelay ? Random.Range(_randomVector.x, _randomVector.y) :_delay);

				if (_positionShake)
				{
					_model.DOShakePosition(_shakeDuration, _strength, _vibrato).SetLink(_model.gameObject);
				}
				else if (_rotationShake)
				{
					_model.DOShakeRotation(_shakeDuration, _strength, _vibrato).SetLink(_model.gameObject);
				}
			}
		}
	}
}