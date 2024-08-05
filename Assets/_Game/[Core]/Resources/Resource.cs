using System;
using _Tools;
using DG.Tweening;
using Gameplay.Characters;
using ScriptableObjects.Classes.Resources;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Resources
{
	public class Resource : MonoBehaviour
	{
		public event Action<Resource> OnArrived;
		public event Action<Resource> OnTaken;
		public event Action<Resource> OnDestroyObject;

		[SerializeField] private SkinnedMeshRenderer _meshRenderer;
		[SerializeField] private MeshFilter _meshFilter;
		[SerializeField] private Transform _modelTransform;
		[SerializeField] private CharacterBase _character;

		[SerializeField] protected Rigidbody _rigidbody;
		[SerializeField] private Collider _collider;
		[SerializeField] private float _rotateDuration;

		private readonly Quaternion _yToZ = Quaternion.Euler(90, 0, 0);
		private Transform _followTarget;
		private float _followForce;

		public bool IsFlying { get; set; }
		public int Count { get; set; }
		public bool IsFell { get; protected set; } = true;

		public ResourceType Type => Data.Type;
		public Mesh Mesh => _meshRenderer != default ? _meshRenderer.sharedMesh : _meshFilter.sharedMesh;
		public Material Material => _meshRenderer.material;
		public Vector3 Bounds => Mesh.bounds.size;
		public ResourceData Data { get; private set; }
		public CharacterBase Character => _character;

		public void SetParent(Transform parent) => transform.SetParent(parent);

		public void SetLocalPosition(Vector3 position) => transform.localPosition = position;

		public void SetLocalEulers(Vector3 eulers) => transform.localEulerAngles = eulers;
		public void SetLocalScale(Vector3 scale) => transform.localScale = scale;
		public void SetPosition(Vector3 position) => transform.position = position;
		public void SetEulers(Quaternion rotation) => transform.rotation = rotation;

		public void Init(ResourceData data)
		{
			IsFell = false;
			IsFlying = false;
			Data = data;
			Count = 1;
			if (_meshFilter != default)
				_meshFilter.sharedMesh = data.ResourceMesh;
			name = data.name;
			SetLocalScale(Vector3.one);
			SetTrigger(false);
			//StartCoroutine(SetColliderMesh());
		}

		public void Fall()
		{
			IsFell = true;
			SetTrigger(true);
		}

		private void Take()
		{
			IsFell = false;
			SetTrigger(true);
			OnTaken?.Invoke(this);
		}

		public void EnableCollider(bool value) => _collider.enabled = value;

		public void SetTrigger(bool enable) => _collider.isTrigger = enable;

		public void SetKinematic(bool enable) => _rigidbody.isKinematic = enable;

		public void RotateAroundY()
		{
			transform.DORotate(Vector3.up * 360, _rotateDuration, RotateMode.FastBeyond360)
			         .SetRelative()
			         .SetEase(Ease.Linear)
			         .SetLoops(-1, LoopType.Restart)
			         .SetLink(gameObject);
			transform.DOMoveY(0.3f, _rotateDuration / 3f)
			         .SetEase(Ease.Linear)
			         .SetLoops(-1, LoopType.Yoyo)
			         .SetLink(gameObject);
		}
		// private IEnumerator SetColliderMesh()
		// {
		// 	while (_collider.sharedMesh != Data.ResourceMesh)
		// 	{
		// 		_collider.sharedMesh = Data.ResourceMesh;
		// 		yield return null;
		// 	}
		// }

		public void MoveTo(Transform goalPos, float duration, Action finished)
		{
			//Take();
			transform.DOKill();
			transform.DOMove(goalPos.position, duration).SetEase(Ease.Linear).SetLink(gameObject);
			//transform.DOScale(goalPos.localScale, duration).SetEase(Ease.Linear)
			transform.DORotate(goalPos.rotation.eulerAngles, duration)
			         .SetEase(Ease.Linear)
			         .SetLink(gameObject)
			         .OnComplete(() => finished?.Invoke());
		}

		public void JumpTo(Transform goalPos, Vector3 goalSize, float height, float duration, Action finished)
		{
			Take();
			transform.DOKill();
			transform.DORotate(goalPos.rotation.eulerAngles, duration).SetEase(Ease.InOutSine).SetLink(gameObject);
			//transform.DOScale(goalSize, duration).SetEase(Ease.InOutSine)
			transform.DOJump(goalPos.position, height, 1, duration)
			         .SetEase(Ease.InOutSine)
			         .SetLink(gameObject)
			         .OnComplete(() => finished?.Invoke());
		}

		public void JumpTo(Vector3 goalPos, float height, float duration, Action finished = default)
		{
			OnTaken?.Invoke(this);
			transform.DOKill();
			transform.DOJump(goalPos, height, 1, duration)
			         .SetEase(Ease.InOutSine)
			         .SetLink(gameObject)
			         .OnComplete(() => finished?.Invoke());
			transform.DOLocalRotate(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)),
			                        duration / 2f,
			                        RotateMode.FastBeyond360)
			         .SetEase(Ease.InOutSine)
			         .SetLink(gameObject)
			         .OnComplete(() => transform.DOLocalRotate(Vector3.zero, duration / 2f, RotateMode.FastBeyond360)
			                                    .SetLink(gameObject)
			                                    .SetEase(Ease.InOutSine));
		}

		public void JumpTo(Vector3 goalPos, Vector3 goalSize, float height, float duration, Action finished)
		{
			Take();
			transform.DOKill();
			//transform.DORotate(goalPos.rotation.eulerAngles, duration).SetEase(Ease.InOutSine);
			var rotation = Vector3.up * transform.eulerAngles.y;
			transform.DORotate(rotation, duration).SetEase(Ease.InOutSine).SetLink(gameObject);
			transform.DOScale(goalSize, duration).SetEase(Ease.InExpo).SetLink(gameObject);
			transform.DOLocalJump(goalPos, height, 1, duration)
			         .SetEase(Ease.InOutSine)
			         .SetLink(gameObject)
			         .OnComplete(() => finished?.Invoke());
		}

		public void WarpTo(Transform goalPos)
		{
			//Take();
			transform.DOKill();
			SetPosition(goalPos.position);
			SetEulers(goalPos.rotation);
			//SetLocalScale(goalPos.localScale);
			OnArrived?.Invoke(this);
		}

		public void MoveToLocal(Vector3 goalPos, float duration = 1f)
		{
			SetLocalPosition(goalPos);
			//transform.DOLocalMove(goalPos, duration);
		}

		public void FlyPhysic(Vector3 direction, float power)
		{
			SetTrigger(false);
			_rigidbody.AddForce(direction * power, ForceMode.VelocityChange);
		}

		public void Release()
		{
			StopAllCoroutines();
			Data = default;
			if (_meshFilter != default)
				_meshFilter.sharedMesh = default;
			//_collider.sharedMesh = default;

			_modelTransform.localScale = Vector3.one;
			ResourcePool.Release(this);
		}

		private void OnDestroy()
		{
			OnDestroyObject?.Invoke(this);
		}
	}
}