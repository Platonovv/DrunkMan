using System.Collections.Generic;
using _Game.DrunkManSpawner.Data;
using _Tools;
using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class DrunkManFactory : MonoBehaviour
	{
		private readonly List<DrunkManData> _drunkManData;

		public DrunkManFactory(List<DrunkManData> drunkManData)
		{
			_drunkManData = drunkManData;
		}

		public DrunkManData GetDrunkManData()
		{
			switch (_drunkManData.GetRandomElement().DrunkManType)
			{
				case DrunkManType.Noting:
					return ScriptableObject.CreateInstance<DrunkManData>();
				case DrunkManType.Normal:
					return ScriptableObject.CreateInstance<DrunkManData>();
				default:
					return default;
			}
		}
	}
}