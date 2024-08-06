using System;
using System.Collections.Generic;
using _Game.DrunkManSpawner.Data;
using _Tools;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class DrunkManHandler : MonoBehaviour
	{
		public event Action<Transform> OnSpawnDrunkMan;

		[Header("Components")]
		[SerializeField] private List<Transform> _spawnPoints;

		private DrunkManFactory _drunkManFactory;
		private List<DrunkManData> _drunkManData;

		public void StartSpawn(List<DrunkManData> drunkManData)
		{
			_drunkManFactory = new DrunkManFactory(drunkManData);
			SpawnRandomDrunkMan();
		}

		private void SpawnRandomDrunkMan()
		{
			_spawnPoints.ForEach(x => x.DestroyChildren());
			var randomElement = _spawnPoints.GetRandomElement();
			var drunkMan = _drunkManFactory.GetDrunkMan();
			drunkMan.SetPosition(randomElement);
			drunkMan.SetParent(randomElement);
			OnSpawnDrunkMan?.Invoke(drunkMan.transform);
		}
	}
}