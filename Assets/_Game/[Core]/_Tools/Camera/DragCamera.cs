using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
		private PointerEventData _pointerEventData;
		private readonly List<RaycastResult> _results = new();
		private GraphicRaycaster _graphicRaycaster;
		private EventSystem _eventSystem;
		private bool _dragCamera;

		private void Start()
		{
			_eventSystem = EventSystem.current;
			_graphicRaycaster = FindObjectOfType<MainCanvasHandler>().GraphicRaycaster;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (IsPointerOverUIElement())
				{
					_dragCamera = false;
					return;
				}

				_dragCamera = true;
				_lastMousePosition = Input.mousePosition;
				return;
			}

			if (!Input.GetMouseButton(0))
				return;

			if (!_dragCamera)
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

		private bool IsPointerOverUIElement()
		{
			_pointerEventData = new PointerEventData(_eventSystem) {position = Input.mousePosition};

			_results.Clear();
			_graphicRaycaster.Raycast(_pointerEventData, _results);

			return _results.Any(result => !result.gameObject.TryGetComponent(out RawImage _));
		}
	}
}