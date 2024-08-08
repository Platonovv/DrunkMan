using System.Collections.Generic;
using _Game.TrapsSpawner.Data;
using UnityEngine;

namespace _Game.TrapsSpawner
{
	public class TrapSpawnerHandler : MonoBehaviour
	{
		[SerializeField] private List<TrapData> _trapsData;
		[SerializeField] private List<TrapSpawner> _trapSpawners;
		
		private TrapFactory _trapFactory;

		private void Awake()
		{
			_trapFactory = new TrapFactory(_trapsData);
			
			foreach (var trapSpawner in _trapSpawners)
			{
				var trap = _trapFactory.GetTrap(trapSpawner.TrapType);
				trap.SetPos(trapSpawner.SpawnPos);
			}
		}
	}
}