using System.Collections.Generic;
using _Game.DrunkManSpawner.Data;
using _Tools;
using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class DrunkManFactory
	{
		private readonly List<DrunkManData> _drunkManData;

		public DrunkManFactory(List<DrunkManData> drunkManData) => _drunkManData = drunkManData;

		public CharacterBase GetDrunkMan()
		{
			var drunkManData = _drunkManData.GetRandomElement();
			switch (drunkManData.DrunkManType)
			{
				case DrunkManType.Noting:
					return default;
				case DrunkManType.Normal:
				case DrunkManType.Slow:
				case DrunkManType.Fast:
					var characterBase = Object.Instantiate(drunkManData.CharacterPrefab);
					characterBase.InitData(drunkManData);
					return characterBase;
				default:
					return default;
			}
		}
	}
}