using UnityEngine;

namespace Utils.ObjectPool
{
	public class RestartRigidbody : MonoBehaviour
	{
		private PoolItem  _poolItem;
		private Rigidbody _rb;

		private void Start()
		{
			_poolItem = GetComponent<PoolItem>();
			_poolItem.OnRestart += Restart;
			_rb = GetComponent<Rigidbody>();
			Restart();
		}

		private void Restart()
		{
			_rb.velocity = Vector3.zero;
		}

		private void OnDestroy()
		{
			_poolItem.OnRestart -= Restart;
		}
	}
}