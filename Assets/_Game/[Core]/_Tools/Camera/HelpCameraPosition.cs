using UnityEngine;

namespace _Tools
{
	public class HelpCameraPosition : MonoBehaviour
	{
		[SerializeField] private Transform _transform;

		private Transform _thisTransform;

		private void Awake()
		{
			_thisTransform = transform;
		}

		private void LateUpdate()
		{
			_thisTransform.position = _transform.position;
			_thisTransform.rotation = _transform.rotation;
		}
	}
}