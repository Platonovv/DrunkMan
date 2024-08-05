using System.Collections;
using _Tools;
using UnityEngine;

namespace Utils.ObjectPool
{
	[RequireComponent(typeof(IPoolable))]
	public class ReleaseAfterDelay : MonoBehaviour
	{
		[SerializeField] private float _delay = 3f;

		private Coroutine _releaseCor;
		private IPoolable _poolItem;

		private void Start()
		{
			_poolItem = GetComponent<IPoolable>();
			if (_poolItem != null)
				_poolItem.OnRestart += RestartObject;
		}

		private void OnEnable()
		{
			RestartObject();
		}

		public void SetDelay(float delay = 0)
		{
			_delay = Mathf.Clamp(delay, 0, float.MaxValue);
		}

		private void RestartObject()
		{
			_releaseCor.Stop(this);
			_releaseCor = StartCoroutine(ReleaseAfterTimeCor());
		}

		private IEnumerator ReleaseAfterTimeCor()
		{
			yield return new WaitForSeconds(_delay);

			_poolItem?.Release();
		}

		private void OnDisable()
		{
			_releaseCor.Stop(this);
		}

		private void OnDestroy()
		{
			if (_poolItem != null)
				_poolItem.OnRestart -= RestartObject;
		}
	}
}