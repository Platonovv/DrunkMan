using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Tools
{
	public class CircularLayout : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Transform _center;
		[Header("Settings")]
		[SerializeField] private float _radius;
		[SerializeField] private float _angleRangePerChild;
		[SerializeField] private float _startAngleShiftPerChild;

		public void ArrangeChildren(List<Transform> children)
		{
			List<Transform> enabledChildren = children.Where(ch => ch.gameObject.activeInHierarchy).ToList();
			float angelRangeTotal = _angleRangePerChild * enabledChildren.Count;
			float startAngle = 120 - enabledChildren.Count * _startAngleShiftPerChild;
			for (int i = 0; i < enabledChildren.Count; i++)
			{
				Transform child = enabledChildren[i];
				float angle = startAngle + i * angelRangeTotal / enabledChildren.Count;
				float radians = angle * Mathf.Deg2Rad;
				Vector3 newPos = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * _radius;
				Vector3 centerLocalPosition = _center.localPosition;
				child.localPosition = centerLocalPosition + newPos;

				// Расчет угла поворота в сторону центральной точки
				Vector3 directionToCenter = (centerLocalPosition - child.localPosition).normalized;
				float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
				child.localRotation = Quaternion.Euler(0, 0, angleToCenter + 90); // -90 для правильного направления
			}
		}
	}
}