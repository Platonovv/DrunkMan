using System;
using UnityEngine;

namespace Utils.ObjectPool
{
	[SelectionBase]
	public class PoolItem : MonoBehaviour, IPoolable
	{
		public int ID { get; private set; }
		public string ContainerName { get; private set; }

		public event Action OnRestart;
		public event Action OnRelease;

		public Transform MyTransform() => transform;

		public virtual void Restart()
		{
			OnRestart?.Invoke();
		}

		public virtual void Retain(int id, string containerName)
		{
			ID = id;
			ContainerName = containerName;
		}

		public virtual void Release(bool disableObject = true)
		{
			OnRelease?.Invoke();
			Pool.Release(ID, this, disableObject);
		}

		public void SetParent(Transform parent) => transform.SetParent(parent);
		public void SetActive(bool active) => gameObject.SetActive(active);
	}
}