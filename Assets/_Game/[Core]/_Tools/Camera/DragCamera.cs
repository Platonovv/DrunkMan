using UnityEngine;

namespace _Tools
{
	public class DragCamera : MonoBehaviour
	{
		[SerializeField] private float _dragSpeed = 2;
		[SerializeField] private Vector3 _minBounds;
		[SerializeField] private Vector3 _maxBounds;

		private Vector3 _dragOrigin;
		private Vector3 _dragVelocity;
		private Vector3 _lastMousePosition;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				_lastMousePosition = Input.mousePosition;
				return;
			}

			if (!Input.GetMouseButton(0))
				return;

			Vector3 currentMousePosition = Input.mousePosition;
			_dragVelocity = (currentMousePosition - _lastMousePosition) / Time.deltaTime;
			_lastMousePosition = currentMousePosition;

			Vector3 move = new Vector3(_dragVelocity.x * -_dragSpeed, 0, _dragVelocity.y * -_dragSpeed)
			               * Time.deltaTime;

			Vector3 newPosition = transform.position + move;
			newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
			newPosition.z = Mathf.Clamp(newPosition.z, _minBounds.z, _maxBounds.z);

			transform.position = newPosition;
		}
	}
}