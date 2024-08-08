using System;
using System.Collections.Generic;
using System.Linq;
using _Game.DrunkManSpawner.Data;
using _Tools;
using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class SpawnCharacterHandler : MonoBehaviour
	{
		public event Action<CharacterBase> OnSpawnDrunkMan;

		[Header("Player")]
		[SerializeField] private List<CharacterData> _playersData;
		[SerializeField] private List<Transform> _spawnPoints;
		[Header("Enemies")]
		[SerializeField] private List<CharacterData> _enemiesData;
		[SerializeField] private List<SpawnPoint> _spawnEnemiesPoints;

		private CharacterFactory _characterFactory;

		private void Awake() => _characterFactory = new CharacterFactory();

		public void SpawnPlayer()
		{
			SpawnRandomPlayer();
			SpawnEnemies();
		}

		private void SpawnEnemies()
		{
			SpawnAllEnemies();
		}

		private void SpawnRandomPlayer()
		{
			_spawnPoints.ForEach(x => x.DestroyChildren());
			var randomElement = _spawnPoints.GetRandomElement();
			var drunkMans = _characterFactory.GetCharacters(_playersData);
			var player = drunkMans.GetRandomElement();
			player.SetPosition(randomElement);
			player.SetParent(randomElement);
			OnSpawnDrunkMan?.Invoke(player);
		}

		private void SpawnAllEnemies()
		{
			_spawnEnemiesPoints.ForEach(x => x.SpawnPos.DestroyChildren());

			var enemies = _characterFactory.GetCharacters(_enemiesData);

			foreach (var enemy in enemies)
			{
				var randomElement = _spawnEnemiesPoints.FirstOrDefault(x => !x.Busy);
				if (randomElement == default)
					continue;

				enemy.SetPosition(randomElement.SpawnPos);
				enemy.SetParent(randomElement.SpawnPos);
				enemy.SetWaypoints(randomElement.EnemyWaypoints);
				enemy.MoveAgent();
				randomElement.SetBusy(true);
			}
		}
	}
}