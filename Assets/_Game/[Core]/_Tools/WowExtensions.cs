using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Object;
using static UnityEngine.Random;

namespace _Tools
{
	public static class WowExtensions
	{
		/// <summary>
		/// Destroy all children
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="immediate"></param>
		/// <returns></returns>
		public static void DestroyChildren(this Transform transform, bool immediate = false)
		{
			if (immediate)
			{
				for (var i = transform.childCount - 1; i >= 0; i--)
				{
					var child = transform.GetChild(i);
					if (DOTween.IsTweening(child)) child.DOKill();
					DestroyImmediate(child.gameObject);
				}
			}
			else
			{
				foreach (Transform child in transform)
				{
					if (DOTween.IsTweening(child)) child.DOKill();
					Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Transform Vector3 (x, y, z) to Vector2 (x, z)
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector2 ToXZVector2(this Vector3 vector) => new(vector.x, vector.z);

		/// <summary>
		/// Transform Vector2 (x, y) to Vector3 (x, 0, z)
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector3 ToXZVector3(this Vector2 vector) => new(vector.x, 0, vector.y);

		/// <summary>
		/// Transform Vector2 (x, y) to Vector3 (x, 0, z)
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector3 ToXZVector3(this Vector2Int vector) => new(vector.x, 0, vector.y);

		/// <summary>
		/// Transform Vector3 (x, y, z) to Vector3 (x, 0, z)
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector3 XZOnly(this Vector3 vector) => new(vector.x, 0, vector.z);

		/// <summary>
		/// Get random element from list
		/// </summary>
		/// <param name="list"></param>
		/// <typeparam name="T">Any type</typeparam>
		/// <returns></returns>
		public static T GetRandomElement<T>(this List<T> list) =>
			list == default || list.Count == 0 ? default : list[Range(0, list.Count)];

		/// <summary>
		/// Get random element from array
		/// </summary>
		/// <param name="list"></param>
		/// <typeparam name="T">Any type</typeparam>
		/// <returns></returns>
		public static T GetRandomElement<T>(this T[] list) =>
			list == default || list.Length == 0 ? default : list[Range(0, list.Length)];

		/// <summary>
		/// Get random integer value between X and Y
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="includeMax"></param>
		/// <returns></returns>
		public static int RandomIntValue(this Vector2Int vector, bool includeMax = false) =>
			Range(vector.x, vector.y + (includeMax ? 1 : 0));

		/// <summary>
		/// Get random integer value between X and Y
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static int RandomIntValue(this Vector2 vector) =>
			Range(Mathf.CeilToInt(vector.x), Mathf.FloorToInt(vector.y) + 1);

		/// <summary>
		/// Get random value between X and Y
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static float RandomValue(this Vector2Int vector) => Range((float) vector.x, vector.y);

		/// <summary>
		/// Get random value between X and Y
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static float RandomValue(this Vector2 vector) => Range(vector.x, vector.y);/// <summary>
		

		/// <summary>
		/// Call action on object.
		/// Return self object for next call.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <typeparam name="T">Any type</typeparam>
		/// <returns></returns>
		public static T With<T>(this T self, Action<T> action)
		{
			action?.Invoke(self);
			return self;
		}

		/// <summary>
		/// Call action on object if condition func returns true.
		/// Return self object for next call.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <param name="condition"></param>
		/// <typeparam name="T">Any type</typeparam>
		/// <returns></returns>
		public static T With<T>(this T self, Action<T> action, Func<bool> condition)
		{
			if (condition()) action?.Invoke(self);
			return self;
		}

		/// <summary>
		/// Call action on object if condition is true.
		/// Return self object for next call.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <param name="condition"></param>
		/// <typeparam name="T">Any type</typeparam>
		/// <returns></returns>
		public static T With<T>(this T self, Action<T> action, bool condition)
		{
			if (condition) action?.Invoke(self);
			return self;
		}

		/// <summary>
		/// Return sign of number.
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static int Sign(this float self) => self > 0 ? 1 : -1;

		/// <summary>
		/// Return sign of bool.
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static int Sign(this bool self) => self ? 1 : -1;

		/// <summary>
		/// Convert bool to int.
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static int ToInt(this bool self) => self ? 1 : 0;

		/// <summary>
		/// Activate the GameObject
		/// </summary>
		/// <param name="target"></param>
		public static void Activate(this Component target) => target.gameObject.SetActive(true);

		/// <summary>
		/// Deactivate the GameObject
		/// </summary>
		/// <param name="target"></param>
		public static void Deactivate(this Component target) => target.gameObject.SetActive(false);

		/// <summary>
		/// Activate the GameObject
		/// </summary>
		/// <param name="target"></param>
		public static void Activate(this GameObject target) => target.gameObject.SetActive(true);

		/// <summary>
		/// Deactivate the GameObject
		/// </summary>
		/// <param name="target"></param>
		public static void Deactivate(this GameObject target) => target.gameObject.SetActive(false);

		/// <summary>
		/// Smooth turn to the target along the y-axis
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <param name="rotationSpeed"></param>
		public static void HorizontalSoftLookAt(this Transform transform, Transform target, float rotationSpeed = 5) =>
			HorizontalSoftLookAt(transform, target.position, rotationSpeed);

		/// <summary>
		/// Smooth turn to the target along the y-axis
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <param name="rotationSpeed"></param>
		public static void HorizontalSoftLookAt(this Transform transform, Vector3 target, float rotationSpeed = 5)
		{
			var position = transform.position;
			target.y = position.y;
			var lookVector = target - position;
			var rotation   = Quaternion.LookRotation(lookVector);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
		}

		/// <summary>
		/// Smooth turn to the target
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <param name="rotationSpeed"></param>
		public static void SoftLookAt(this Transform transform, Transform target, float rotationSpeed = 5) =>
			SoftLookAt(transform, target.position, rotationSpeed);

		/// <summary>
		/// Smooth turn to the target
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <param name="rotationSpeed"></param>
		public static void SoftLookAt(this Transform transform, Vector3 target, float rotationSpeed = 5)
		{
			var position   = transform.position;
			var lookVector = target - position;
			var rotation   = Quaternion.LookRotation(lookVector);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
			var transformLocalEulerAngles = transform.localEulerAngles;
			transformLocalEulerAngles.z = 0f;
			transform.localEulerAngles = transformLocalEulerAngles;
		}

		/// <summary>
		/// Turn to the target along the y-axis
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		public static void HorizontalLookAt(this Transform transform, Transform target) =>
			HorizontalLookAt(transform, target.position);

		/// <summary>
		/// Turn to the target along the y-axis
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		public static void HorizontalLookAt(this Transform transform, Vector3 target)
		{
			var position = transform.position;
			target.y = position.y;
			var lookVector = target - position;
			var rotation   = Quaternion.LookRotation(lookVector);
			transform.rotation = rotation;
		}

		/// <summary>
		/// Distance to transform
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float DistanceTo(this Transform transform, Transform target) =>
			(transform.position - target.position).magnitude;

		/// <summary>
		/// Distance to position
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static float DistanceTo(this Transform transform, Vector3 targetPosition) =>
			(transform.position - targetPosition).magnitude;

		/// <summary>
		/// Distance to transform
		/// </summary>
		/// <param name="component"></param>
		/// <param name="targetTransform"></param>
		/// <returns></returns>
		public static float SqrDistanceTo(this Component component, Transform targetTransform) =>
			(component.transform.position - targetTransform.position).sqrMagnitude;

		/// <summary>
		/// Distance to position
		/// </summary>
		/// <param name="component"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static float SqrDistanceTo(this Component component, Vector3 targetPosition) =>
			(component.transform.position - targetPosition).sqrMagnitude;

		/// <summary>
		/// Distance to position
		/// </summary>
		/// <param name="position"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static float DistanceTo(this Vector3 position, Vector3 targetPosition) =>
			(position - targetPosition).magnitude;

		/// <summary>
		/// Is agent reached the destination
		/// </summary>
		/// <param name="agent"></param>
		/// <returns></returns>
		public static bool IsReached(this NavMeshAgent agent) =>
			agent.transform.DistanceTo(agent.destination) <= agent.stoppingDistance;

		/// <summary>
		/// Is agent reached the position
		/// </summary>
		/// <param name="agent"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsReached(this NavMeshAgent agent, Vector3 target) =>
			agent.transform.DistanceTo(target) <= agent.stoppingDistance;

		/// <summary>
		/// Extension method to check if a layer is in a layer mask
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public static bool Contains(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		/// <summary>
		/// Check that point intersect with rectangle (XZ coordinates)
		/// </summary>
		/// <param name="pointPosition"></param>
		/// <param name="intersectionCenter"></param>
		/// <param name="borderX"></param>
		/// <param name="borderZ"></param>
		/// <returns>Return true if point inside rectangle, else false.</returns>
		public static bool IsIntersection(this Vector3 pointPosition, Vector3 intersectionCenter, float borderX,
																			float borderZ)
		{
			return (pointPosition.x <= intersectionCenter.x + borderX / 2 &&
						  pointPosition.x >= intersectionCenter.x - borderX / 2) &&
						 (pointPosition.z <= intersectionCenter.z + borderZ / 2 &&
						  pointPosition.z >= intersectionCenter.z - borderZ / 2);
		}

		/// <summary>
		/// Get random position (XZ only) inside rectangle
		/// </summary>
		/// <param name="center"></param>
		/// <param name="borderX"></param>
		/// <param name="borderZ"></param>
		/// <returns></returns>
		public static Vector3 GenerateRandomPosition(Vector3 center, float borderX, float borderZ)
		{
			return new Vector3(
				center.x + Range(-borderX / 2, borderX / 2),
				center.y,
				center.z + Range(-borderZ / 2, borderZ / 2));
		}

		/// <summary>
		/// Is the number even 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static bool IsEven(this int number) => number % 2 == 0;

		/// <summary>
		/// Clamp the value from min to max
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static void Clamp(this ref float value, float min, float max) => value = Mathf.Clamp(value, min, max);

		/// <summary>
		/// StopCoroutine
		/// </summary>
		/// <param name="coroutine"></param>
		/// <param name="owner">"Usually (this)"</param>
		public static void Stop(this Coroutine coroutine, MonoBehaviour owner)
		{
			if (coroutine != default) owner.StopCoroutine(coroutine);
		}

		/// <summary>
		/// Convert number to four char string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToShortString(this int value) => ((ulong) value).ToShortString();

		/// <summary>
		/// Convert number to four char string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToShortString(this ulong value)
		{
			if (value < 10_000) return value.ToString();

			var letterArray   = new[] {"", "K", "M", "B", "t", "q", "Q", "s", "S", "O", "N", "d", "U", "D", "T"};
			var moneyString   = value.ToString();
			var thousandCount = (moneyString.Length - 1) / 3;
			if (thousandCount >= letterArray.Length) throw new ArgumentOutOfRangeException();

			var letter = letterArray[thousandCount];
			return moneyString[..^(thousandCount * 3)] + letter;
		}

		/// <summary>
		/// Dot product
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float Dot(this Transform origin, Transform target)
		{
			return Vector3.Dot(origin.forward, (target.transform.position - origin.position).XZOnly().normalized);
		}
	}
}