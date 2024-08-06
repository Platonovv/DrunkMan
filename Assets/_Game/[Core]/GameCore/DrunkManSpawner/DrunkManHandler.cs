using System;
using System.Collections.Generic;
using _Game.DrunkManSpawner.Data;
using _Tools;
using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class DrunkManHandler : MonoBehaviour
	{
		public event Action<Transform> OnSpawnDrunkMan;

		[Header("Components")]
		[SerializeField] private List<Transform> _spawnPoints;

		private CharacterBase _currentDrunkMan;
		private DrunkManFactory _drunkManFactory;
		private List<DrunkManData> _drunkManData;

		public void StartSpawn(List<DrunkManData> drunkManData)
		{
			_drunkManFactory = new DrunkManFactory(drunkManData);
			SpawnRandomDrunkMan();
		}

		private void SpawnRandomDrunkMan()
		{
			_currentDrunkMan = _drunkManFactory.GetDrunkMan();
			_currentDrunkMan.SetPosition(_spawnPoints.GetRandomElement());
			OnSpawnDrunkMan?.Invoke(_currentDrunkMan.transform);
		}
	}
}