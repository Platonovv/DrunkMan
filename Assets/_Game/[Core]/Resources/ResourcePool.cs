using System.Collections.Generic;
using System.Linq;
using _Tools;
using ScriptableObjects.Classes.Resources;
using UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Resources
{
	public class ResourcePool : Singleton<ResourcePool>
	{
		//TODO: Replace with ObjectPool
		[SerializeField] private Resource _resourcePrefab;

		[SerializeField]
		private List<ResourceData> _data;

		private static readonly Dictionary<ResourceType, ResourceData> resourceData = new();

		private static readonly ObjectPool<Resource> allResources
			= new(CreateResource, GetResource, ReleaseResource, DestroyResource);

		private static Resource CreateResource()
		{
			return Instantiate(Instance._resourcePrefab, Instance.transform);
		}

		private static void GetResource(Resource resource)
		{
			resource.SetParent(null);
			resource.gameObject.SetActive(true);
		}

		private static void ReleaseResource(Resource resource)
		{
			var resourceTransform = resource.transform;
			resourceTransform.SetParent(Instance.transform);
			resourceTransform.localPosition = Vector3.zero;
			resource.gameObject.SetActive(false);
		}

		private static void DestroyResource(Resource resource)
		{
			Destroy(resource.gameObject);
		}

		protected override void Awake()
		{
			base.Awake();
			foreach (var data in _data)
			{
				resourceData.Add(data.Type, data);
			}
		}

		public static bool Get(ResourceType resourceType, out Resource resource)
		{
			if (Instance == null)
			{
				Debug.LogError("ResourcePool Instance Not Found!");
				resource = null;
				return false;
			}

			if (Instance._data.Count == 0)
			{
				Debug.LogError("Resource Data Not Found!", Instance);
				resource = null;
				return false;
			}

			resource = allResources.Get()
			                       .With(x => x.name = $"{resourceType}_{allResources.CountAll}")
			                       .With(x => x.Init(resourceData[resourceType]));
			return true;
		}

		public static void Release(Resource resource)
		{
			allResources.Release(resource);
		}

		public static void Clear()
		{
			//foreach (var resource in Instance.transform.GetComponentsInChildren<Resource>()) resource.Release();
			Instance.transform.DestroyChildren();
			allResources.Clear();
		}

		public static int GetPrice(ResourceType type)
		{
			return resourceData.First(x => x.Value.Type == type).Value.Price;
		}

		public static Sprite GetIcon(ResourceType type)
		{
			return resourceData.First(x => x.Value.Type == type).Value.ResourceIcon;
		}
	}
}