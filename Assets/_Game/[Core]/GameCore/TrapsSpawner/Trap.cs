using UnityEngine;

namespace _Game.TrapsSpawner
{
	public class Trap : MonoBehaviour
	{
		public void SetPos(Transform spawnPos)
		{
			transform.SetParent(spawnPos);
			transform.position = spawnPos.position;
		}
	}
}