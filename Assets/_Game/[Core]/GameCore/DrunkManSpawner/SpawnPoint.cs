using System.Collections.Generic;
using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class SpawnPoint : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private List<Transform> _enemyWaypoints;
		[SerializeField] private Transform _spawnPos;
		[SerializeField] private bool _busy;
		
		public Transform SpawnPos => _spawnPos;
		public List<Transform> EnemyWaypoints => _enemyWaypoints;
		public bool Busy => _busy;
		public void SetBusy(bool busy) => _busy = busy;
	}
}