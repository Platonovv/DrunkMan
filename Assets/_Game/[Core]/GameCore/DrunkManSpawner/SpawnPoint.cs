using UnityEngine;

namespace _Game.DrunkManSpawner
{
	public class SpawnPoint : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPos;
		[SerializeField] private bool _busy;
		
		public Transform SpawnPos => _spawnPos;
		public bool Busy => _busy;
		public void SetBusy(bool busy) => _busy = busy;
	}
}