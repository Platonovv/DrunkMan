using UnityEngine;

namespace _Tools
{
	public class MiniMapCameraHelper : MonoBehaviour
	{
		private Transform _thisTransform;
		private Transform _follow;

		public void SetFollow(Transform follow) => _follow = follow;

		private void Awake() => _thisTransform = transform;

		private void LateUpdate()
		{
			if (_follow == default)
				return;

			_thisTransform.position = new Vector3(_follow.position.x, _thisTransform.position.y, _follow.position.z);
		}
	}
}