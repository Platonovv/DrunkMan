using System;
using System.Collections.Generic;
using Common;
using Unity.Profiling;

namespace UI
{
	public static class ResourceHandler
	{
		public static event Action<ResourceType, int> OnValueSet;
		public static event Action<ResourceType, int> OnValueAdded;
		public static event Action<ResourceType, int> OnValueSubtracted;
		public static event Action<ResourceType, int> OnValueChanged;

		private static readonly Dictionary<ResourceType, IntDataValueSavable> resources = new();

		public static void AddResource(ResourceType type, int addedValue, bool autoSave = false)
		{
			if (addedValue < 0)
				throw new ArgumentOutOfRangeException(null, "ArgumentOutOfRange_BadAddedValue");

			if (!resources.ContainsKey(type))
				resources.Add(type, new IntDataValueSavable(type.ToString()));

			OnValueAdded?.Invoke(type, addedValue);
			var value = resources[type].Value += addedValue;
			OnValueChanged?.Invoke(type, value);
			if (autoSave)
				SaveData(type);
			ProfilerRecorder.StartNew(ProfilerCategory.Scripts, "Custom");
		}

		public static bool TrySubtractResource(ResourceType type, int subtractValue, bool autoSave = false)
		{
			if (subtractValue < 0)
				throw new ArgumentOutOfRangeException(null, "ArgumentOutOfRange_BadSubtractedValue");

			if (!resources.ContainsKey(type))
				resources.Add(type, new IntDataValueSavable(type.ToString()));
			var value = resources[type].Value;
			if (value < subtractValue)
				return false;

			OnValueSubtracted?.Invoke(type, subtractValue);
			resources[type].Value = value -= subtractValue;
			OnValueChanged?.Invoke(type, value);
			if (autoSave)
				SaveData(type);
			return true;
		}

		public static void SaveData()
		{
			foreach (var valueSavable in resources)
			{
				valueSavable.Value.Save();
			}
		}

		public static void SaveData(ResourceType type)
		{
			if (resources.TryGetValue(type, out var resource))
			{
				resource.Save();
			}
		}

		public static int GetResourceCount(ResourceType type)
		{
			if (!resources.ContainsKey(type))
				resources.Add(type, new IntDataValueSavable(type.ToString()));
			return resources[type].Value;
		}

		private static void LoadData(ResourceType type)
		{
			if (!resources.ContainsKey(type))
				resources.Add(type, new IntDataValueSavable(type.ToString()));
			OnValueSet?.Invoke(type, resources[type].Value);
		}

		public static void ResetAllData()
		{
			foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
			{
				TrySubtractResource(resourceType, GetResourceCount(resourceType), true);
			}
		}

		public static void LoadAllData()
		{
			foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
			{
				LoadData(resourceType);
			}
		}
	}

	public enum ResourceType
	{
		Money,
		Prisoner,
		Star,
	}
}