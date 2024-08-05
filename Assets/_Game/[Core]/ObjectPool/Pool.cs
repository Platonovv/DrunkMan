using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.ObjectPool
{
	public class Pool : MonoBehaviour
	{
		private static GameObject PoolGameObject { get; set; }
		private static Pool _instance;

		private static readonly Dictionary<int, Queue<IPoolable>> poolItems = new();
		private static readonly Dictionary<int, Transform> containers = new();
		private static readonly HashSet<IPoolable> usedItems = new();
		private Transform CachedTransform => _cachedTransform == null ? _cachedTransform = transform : _cachedTransform;

		private Transform _cachedTransform;

		private void Awake()
		{
			if (_instance != null)
				Destroy(this);

			_instance = this;
			PoolGameObject = gameObject;
		}

		private void OnDestroy()
		{
			poolItems.Clear();
			containers.Clear();
			usedItems.Clear();
		}

		public static Pool Instance
		{
			get
			{
				if (_instance != default)
					return _instance;

				PoolGameObject = new GameObject("###_MAIN_POOL_###");
				_instance = PoolGameObject.AddComponent<Pool>();

				return _instance;
			}
		}

		public static T Get<T>(T prefab, Vector3 position, Transform parent = null)
			where T : UnityEngine.Object, IPoolable
		{
			var id = prefab.GetInstanceID();
			var queue = GetQueue(id);
			var container = GetContainer(id);
			if (queue.Count > 0)
			{
				var pooledItem = queue.Dequeue();
				var pooledItemTransform = pooledItem.MyTransform();
				if (parent != default)
					pooledItemTransform.parent = parent;

				pooledItemTransform.position = position;
				//pooledItemTransform.localScale = Vector3.one;
				pooledItem.MyTransform().gameObject.SetActive(true);
				pooledItem.Restart();
				usedItems.Add(pooledItem);

				UpdateContainerName(container, queue.Count, prefab.name);
				return (T) pooledItem;
			}

			UpdateContainerName(container, 0, prefab.name);

			var newParent = parent == default ? container : parent;
			return InstantiateObject(prefab, position, newParent, id);
		}

		private static T InstantiateObject<T>(T prefab,
		                                      Vector3 position,
		                                      Transform newParent,
		                                      int id,
		                                      bool activate = true) where T : UnityEngine.Object, IPoolable
		{
			var instance = Instantiate(prefab, position, prefab.MyTransform().rotation, newParent);
			//instance.transform.localScale = Vector3.one;
			instance.name = prefab.name;
			instance.Retain(id, prefab.name);
			instance.SetActive(activate);
			usedItems.Add(instance);

			return instance;
		}

		public static void Release<T>(int id, T poolItem, bool disableObject = true)
			where T : UnityEngine.Object, IPoolable
		{
			//if (!usedItems.Contains(poolItem)) return;

			var queue = GetQueue(id);
			if (!queue.Contains(poolItem))
				queue.Enqueue(poolItem);
			usedItems.Remove(poolItem);

			var container = GetContainer(id);
			poolItem.SetParent(container);
			UpdateContainerName(container, queue.Count, poolItem.ContainerName);

			if (disableObject)
			{
				poolItem.SetActive(false);
			}

			//Destroy(poolItem.gameObject);
		}

		public static void ReleaseAll()
		{
			foreach (var item in usedItems.ToList())
			{
				item.Release();
			}
		}

		public static void ReleaseAll(PoolItem item) => ReleaseAll(item.GetInstanceID());

		public static void ReleaseAll(int id)
		{
			foreach (var item in usedItems.Where(x => x.ID == id).ToList())
			{
				item.Release();
			}
		}

		private static Queue<IPoolable> GetQueue(int id)
		{
			if (poolItems.TryGetValue(id, out var queue))
				return queue;

			queue = new Queue<IPoolable>();
			poolItems.Add(id, queue);

			return queue;
		}

		public static Transform GetContainer(int id)
		{
			if (containers.TryGetValue(id, out var container))
			{
				return container;
			}

			container = new GameObject().transform;
			container.parent = Instance.CachedTransform;
			containers.Add(id, container);

			return container;
		}

		private static void UpdateContainerName(Transform container, int pooled, string name = default)
		{
#if UNITY_EDITOR
			var newName = name ?? container.name;
			if (name != default)
				container.name = $"{newName}\t{pooled}/{container.childCount}";
#endif
		}
	}

	public interface IPoolable
	{
		public int ID { get; }
		public string ContainerName { get; }
		public event Action OnRestart;
		public event Action OnRelease;
		Transform MyTransform();
		public void Restart();
		public void Retain(int id, string containerName);
		public void Release(bool disableObject = true);
		public void SetParent(Transform parent);
		public void SetActive(bool active);
	}
}