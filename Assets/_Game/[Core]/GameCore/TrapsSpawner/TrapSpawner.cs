using _Game.TrapsSpawner.Data;
using UnityEngine;

namespace _Game.TrapsSpawner
{
	public class TrapSpawner : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPos;
		[SerializeField] private TrapType _trapType;

		public TrapType TrapType => _trapType;
		public Transform SpawnPos => _spawnPos;
	}
}