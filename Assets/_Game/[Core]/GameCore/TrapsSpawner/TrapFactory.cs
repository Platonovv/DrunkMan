using System.Collections.Generic;
using System.Linq;
using _Game.TrapsSpawner.Data;
using Object = UnityEngine.Object;

namespace _Game.TrapsSpawner
{
	public class TrapFactory
	{
		private readonly List<TrapData> _trapsData;

		public TrapFactory(List<TrapData> trapsData) => _trapsData = trapsData;

		public Trap GetTrap(TrapType trapType)
		{
			Trap trap = default;
			switch (trapType)
			{
				case TrapType.Noting:
					break;
				case TrapType.FloorTrap:
					var trapData = _trapsData.FirstOrDefault(x => x.TrapType == trapType);
					if (trapData != default)
						trap = Object.Instantiate(trapData.TrapPrefab);

					break;
			}

			return trap;
		}
	}
}