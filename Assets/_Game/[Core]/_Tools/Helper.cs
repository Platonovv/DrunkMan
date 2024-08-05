using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Tools
{
	public static class Helper
	{
#region PerformScale

		public static void PerformCuteScale(Vector3 startScale,
		                                    Vector3 endScale,
		                                    Transform transformToScale,
		                                    float duration,
		                                    Action doOnComplete = null,
		                                    Ease ease = Ease.Linear)
		{
			transformToScale.localScale = startScale;

			if (endScale == Vector3.zero)
				transformToScale.DOScale(startScale * 1.45f, duration * 0.5f)
				                .SetEase(ease)
				                .SetUpdate(true)
				                .OnComplete(() => transformToScale.DOScale(endScale, duration * 0.5f))
				                .SetEase(ease)
				                .SetUpdate(true)
				                .OnComplete(() => doOnComplete?.Invoke());
			else
				transformToScale.DOScale(endScale * 1.45f, duration * 0.5f)
				                .SetEase(ease)
				                .SetUpdate(true)
				                .OnComplete(() => transformToScale.DOScale(endScale, duration * 0.5f))
				                .SetEase(ease)
				                .SetUpdate(true)
				                .OnComplete(() => doOnComplete?.Invoke());
		}

#endregion

#region GetRandomPointOnPlane

		public static Vector3 GetRandomPointOnPlane(this Transform target, float radius)
		{
			var insideUnitCircle = Random.insideUnitCircle;
			var direction = new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y);
			return target.position + direction * radius;
		}

#endregion

#region ListShuffle

		public static List<T> ShuffleList<T>(List<T> list)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = Random.Range(0, n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}

			return list;
		}

#endregion

#region MoneyShrinker

		public static string MoneyString(this int moneyValue)
		{
			if (moneyValue < 0)
				return "0";

			if (moneyValue < 10_000)
				return moneyValue.ToString();

			var moneyString = moneyValue.ToString();
			var thousandCount = (moneyString.Length - 1) / 3;
			switch (thousandCount)
			{
				case 1:
					moneyString = moneyString.Substring(0, moneyString.Length - 3) + "K";
					break;
				case 2:
					moneyString = moneyString.Substring(0, moneyString.Length - 6) + "M";
					break;
				case 3:
					moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
					break;
				default:
					moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
					break;
			}

			return moneyString;
		}

#endregion

#region CameraMain

		private static UnityEngine.Camera _camera;

		public static UnityEngine.Camera Camera
		{
			get
			{
				if (_camera == default)
					_camera = UnityEngine.Camera.main;

				return _camera;
			}
		}

#endregion

#region WaitDictionary

		private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
		private static readonly Dictionary<float, WaitForSecondsRealtime> WaitRealTimeDictionary = new();

		public static WaitForSeconds GetWait(float time)
		{
			if (WaitDictionary.TryGetValue(time, out var wait))
				return wait;

			WaitDictionary[time] = new WaitForSeconds(time);
			return WaitDictionary[time];
		}

		public static WaitForSecondsRealtime GetWaitRealTime(float time)
		{
			return new WaitForSecondsRealtime(time);
		}

		public static IEnumerator WaitCoroutine(float duration, Action finished = null)
		{
			yield return GetWait(duration);

			finished?.Invoke();
		}

		public static IEnumerator WaitRealTime(float duration, Action finished = null)
		{
			yield return GetWaitRealTime(duration);

			finished?.Invoke();
		}

		public static async void DelayForSeconds(float delay, Action doAfterDelay)
		{
			int milliseconds = (int) (delay * 1000);
			await Task.Delay(milliseconds);
			doAfterDelay?.Invoke();
		}

#endregion

#region PointerOverUI

		private static PointerEventData _eventDataCurrentPos;
		private static List<RaycastResult> _result;

		public static bool IsPointerOverUI()
		{
			_eventDataCurrentPos = new PointerEventData(EventSystem.current)
			{
				position = UnityEngine.Input.mousePosition
			};
			_result = new List<RaycastResult>();
			EventSystem.current.RaycastAll(_eventDataCurrentPos, _result);
			return _result.Count > 0;
		}

#endregion

		public static void SetSize(this RectTransform trans, Vector2 newSize)
		{
			Vector2 oldSize = trans.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			Vector2 pivot = trans.pivot;
			trans.offsetMin -= new Vector2(deltaSize.x * pivot.x, deltaSize.y * pivot.y);
			trans.offsetMax += new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - pivot.y));
		}

		public static IEnumerable<T> GetEnumValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();

		public static void JumpToTransform(this Transform origin,
		                                   Transform target,
		                                   Vector3 goalSize,
		                                   float height,
		                                   float duration,
		                                   Action doOnComplete)
		{
			Vector3 rndJump = Random.insideUnitSphere;
			Vector3 startPos = origin.position;

			origin.DOKill();
			origin.DOScale(goalSize, duration * 1.1f).SetEase(Ease.InOutBounce).SetLink(origin.gameObject);
			origin.DOShakeRotation(duration, 50f, 2, 50f, true, ShakeRandomnessMode.Harmonic)
			      .SetLink(origin.gameObject);

			DOTween.To(() => 0.0f, ChangePosition, 1.0f, duration)
			       .OnComplete(doOnComplete.Invoke)
			       .SetLink(origin.gameObject);

			void ChangePosition(float percent)
			{
				Vector3 targetPosition = target.position;
				Vector3 thirdPoint = new Vector3(targetPosition.x + rndJump.x,
				                                 targetPosition.y + height + rndJump.y,
				                                 targetPosition.z + rndJump.z);
				Vector3 firstLerp = Vector3.Lerp(startPos, thirdPoint, percent);
				Vector3 secondLerp = Vector3.Lerp(thirdPoint, targetPosition, percent);
				origin.position = Vector3.Lerp(firstLerp, secondLerp, percent);
			}
		}

		public static void JumpToVector3(this Transform origin,
		                                 Vector3 target,
		                                 Vector3 goalSize,
		                                 float height,
		                                 float duration,
		                                 Action doOnComplete)
		{
			Vector3 rndJump = Random.insideUnitSphere;
			Vector3 startPos = origin.position;

			origin.DOKill();
			origin.DOScale(goalSize, duration * 1.1f).SetEase(Ease.InOutBounce).SetLink(origin.gameObject);
			origin.DOShakeRotation(duration, 50f, 2, 50f, true, ShakeRandomnessMode.Harmonic)
			      .SetLink(origin.gameObject);
			DOTween.To(() => 0.0f, ChangePosition, 1.0f, duration)
			       .OnComplete(doOnComplete.Invoke)
			       .SetLink(origin.gameObject);

			void ChangePosition(float percent)
			{
				Vector3 targetPosition = target;
				Vector3 thirdPoint = new Vector3(targetPosition.x + rndJump.x,
				                                 targetPosition.y + height + rndJump.y,
				                                 targetPosition.z + rndJump.z);
				Vector3 firstLerp = Vector3.Lerp(startPos, thirdPoint, percent);
				Vector3 secondLerp = Vector3.Lerp(thirdPoint, targetPosition, percent);
				origin.position = Vector3.Lerp(firstLerp, secondLerp, percent);
			}
		}

		public static Vector3 CalculateWorldPosition(this Vector3 position)
		{
			//if the point is behind the camera then project it onto the camera plane
			Vector3 camNormal = Camera.transform.forward;
			Vector3 vectorFromCam = position - Camera.transform.position;
			float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
			if (camNormDot <= 0f)
			{
				//we are beind the camera, project the position on the camera plane
				float camDot = Vector3.Dot(camNormal, vectorFromCam);
				Vector3
					proj = (camNormal * (camDot * 1.01f)); //small epsilon to keep the position infront of the camera
				position = Camera.transform.position + (vectorFromCam - proj);
			}

			return position;
		}

		public static float GeometricProgression(float startValue, float coeff, int degree)
			=> startValue * (Mathf.Pow(coeff, degree));

		public static float NiceGeometricProgression(float startValue, float coeff, int degree, float startValueShift)
			=> startValue * (Mathf.Pow(coeff, degree)) + startValueShift - startValue;

		private const int MAX_FLY_SPAWN_COUNT = 12;
		private const int RANDOM_SPAWN_RADIUS = 60;

		/*public static void TransformSequence(Vector3 spawnPosition,
		                                       Transform counter,
		                                       Transform prefab,
		                                       Sprite resourceSprite,
		                                       int value,
		                                       bool pool,
		                                       Transform parent = default,
		                                       bool forceScreenCoordinates = false,
		                                       bool directionFlyLeft = false)
		{
			int resourceCount = Mathf.Min(value, MAX_FLY_SPAWN_COUNT);
			int defaultIncreaseValue = Mathf.FloorToInt((float) value / resourceCount);
			int lastIncreaseValue = value - (defaultIncreaseValue * resourceCount) + defaultIncreaseValue;

			for (var i = 0; i < resourceCount; i++)
			{
				Transform target = parent == default ? counter.Transform : parent;
				Transform Transform = default;
				Transform.ContainedResourceCount = i == resourceCount - 1 ? lastIncreaseValue : defaultIncreaseValue;
				Transform.SetSprite(resourceSprite);
				Transform flyTransform = Transform.transform;
				flyTransform.DOKill();
				flyTransform.localScale = Vector3.zero;
				flyTransform.position = spawnPosition + (Vector3) (Random.insideUnitCircle * RANDOM_SPAWN_RADIUS);
				float delay = i == 0 ? 0 : Random.Range(0.1f, 0.25f);
				Vector3 targetPosition = forceScreenCoordinates
					                         ? Camera.WorldToScreenPoint(counter.Transform.position)
					                         : counter.Transform.position;
				Sequence flySequence = DOTween.Sequence();
				flySequence.SetUpdate(true);
				flySequence.AppendInterval(delay);
				flySequence.Append(flyTransform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc));

				var directionFly = directionFlyLeft ? -1f : 1f;
				Vector3 direction = targetPosition - spawnPosition;
				Vector3 middlePoint = Vector3.Lerp(targetPosition, spawnPosition, Random.Range(0.35f, 0.75f));
				Vector3 perpendicular = Vector3.Cross(directionFly * direction, Vector3.forward);
				Vector3 shift = middlePoint + perpendicular / 4f;

				// Create the arc path
				Vector3[] path = new Vector3[3];
				path[0] = spawnPosition;
				path[1] = shift;
				path[2] = targetPosition;

				flySequence.Join(flyTransform.DOPath(path, 0.6f, PathType.CatmullRom).SetEase(Ease.Linear));

				flySequence.Append(flyTransform.DOScale(0f, 0.2f).SetEase(Ease.InCirc));
				flySequence.OnComplete(() =>
				{
					counter.UpdateResourceUiView(Transform.ContainedResourceCount);
					Transform.Release();
				});
			}
		}*/

		public static void DrawBound(Bounds bounds, float timer, Color color)
		{
#if UNITY_EDITOR
			Debug.DrawLine(bounds.min, bounds.max, color, timer);
#endif
		}

		public static void DrawDebug(Vector3 worldPos, float radius, float timer, Color color)
		{
#if UNITY_EDITOR
			Debug.DrawRay(worldPos, radius * Vector3.up, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.down, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.left, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.right, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.forward, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.back, color, timer);
#endif
		}

	/*	public static T GetRandomWithWeights<T>(List<T> rarityChances) where T : IHasWeight
		{
			List<T> availableChances = new List<T>(rarityChances);
			availableChances.Randomize();
			float totalChance = availableChances.Sum(e => e.Weight);
			float randomValue = Random.Range(0, totalChance);
			int currentIndex = 0;
			while (randomValue >= 0)
			{
				randomValue -= availableChances[currentIndex].Weight;
				currentIndex++;
			}

			currentIndex = Mathf.Clamp(currentIndex - 1, 0, availableChances.Count - 1);
			return availableChances[currentIndex];
		}

		public static List<T> GetRandomListWithWeights<T>(List<T> rarityChances, int returnCount) where T : IHasWeight
		{
			if (returnCount > rarityChances.Count)
			{
				Debug.Log($"returnCount > rarityChances.Count in method {nameof(GetRandomListWithWeights)}");
				returnCount = rarityChances.Count;
			}

			List<T> availableChances = new List<T>(rarityChances);
			List<T> selectedChances = new List<T>();
			while (selectedChances.Count < returnCount)
			{
				availableChances.Randomize();
				float totalChance = availableChances.Sum(e => e.Weight);
				float randomValue = Random.Range(0, totalChance);
				int currentIndex = 0;
				while (randomValue >= 0)
				{
					randomValue -= availableChances[currentIndex].Weight;
					currentIndex++;
				}

				currentIndex = Mathf.Clamp(currentIndex - 1, 0, availableChances.Count - 1);
				selectedChances.Add(availableChances[currentIndex]);
				availableChances.RemoveAt(currentIndex);
			}

			return selectedChances;
		}*/

		//TODO - встваить дикшинари для переиспользования строк!
		/*public static string GetTimeTextFromSpan(TimeSpan timeSpan, TimerDisplay timerDisplay)
		{
			string timeText;
			switch (timerDisplay)
			{
				case TimerDisplay.Seconds:
					timeText = $"{timeSpan.Seconds:D2}s";
					break;
				case TimerDisplay.Minutes:
					timeText = $"{timeSpan.Minutes:D2}m";
					break;
				case TimerDisplay.Hours:
					timeText = $"{timeSpan.Hours:D2}h";
					break;
				case TimerDisplay.MinutesSeconds:
					timeText = $"{timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
					break;
				case TimerDisplay.HoursMinutesSeconds:
					timeText = $"{timeSpan.Hours:D2}h {timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
					break;
				case TimerDisplay.HoursMinutes:
					timeText = $"{timeSpan.Hours:D2}h {timeSpan.Minutes:D2}m";
					break;
				case TimerDisplay.DaysHours:
					timeText = $"{timeSpan.Days:D2}d {timeSpan.Hours:D2}h";
					break;
				default:
					timeText = $"{timeSpan.Seconds:D2}s";
					break;
			}

			return timeText;
		}*/

		public static IEnumerator WaitFrame(Action action)
		{
			yield return default;

			action.Invoke();
		}

		public static (int, int) SelectRandomIndex(int maxCount, int lastPlayedClipIndex, int repeatTimes)
		{
			int randomIndex = Random.Range(0, maxCount);
			if (randomIndex == lastPlayedClipIndex)
			{
				repeatTimes++;
				if (repeatTimes > 1)
				{
					if (randomIndex == maxCount - 1)
					{
						randomIndex = Random.Range(0, maxCount - 1);
					}
					else
					{
						randomIndex += 1;
					}
				}
			}
			else
			{
				repeatTimes = 0;
			}

			return (randomIndex, repeatTimes);
		}

		public static Vector3 ExpDecay(Vector3 a, Vector3 b, float decay, float dt)
		{
			return b + (a - b) * Mathf.Exp(-decay * dt);
		}

		public static (float Lower, float Upper) FindNearestNumbers(List<float> numbers, float target)
		{
			if (numbers == null | numbers.Count == 0)
			{
				throw new ArgumentException("Список чисел не может быть пустым.");
			}

			float lower = 0f;
			float upper = 0f;

			foreach (var number in numbers)
			{
				if (number < target)
				{
					if (lower == null | number > lower)
					{
						lower = number;
					}
				}
				else if (number > target)
				{
					if (upper == null | number < upper)
					{
						upper = number;
					}
				}
			}

			return (lower, upper);
		}

		private static StructNearestNumbers _nearestStruct;

		public static ref StructNearestNumbers NearestNumbers(List<float> numbers, float target)
		{
			numbers = numbers.OrderBy(x => x).ToList();
			_nearestStruct.Lower = numbers.First();
			_nearestStruct.Upper = numbers.Last();

			for (var i = 0; i < numbers.Count; i++)
			{
				var number = numbers[i];
				if (number < target)
				{
					_nearestStruct.Lower = number;
					_nearestStruct.IndexOfLower = i;
				}
				else if (number >= target && number < _nearestStruct.Upper)
				{
					_nearestStruct.Upper = number;
				}
			}

			return ref _nearestStruct;
		}

		public static float SegmentReverseLerp(StructNearestNumbers structNearestNumbers, float currentValue)
		{
			var range = structNearestNumbers.Upper - structNearestNumbers.Lower;
			var position = currentValue - structNearestNumbers.Lower;
			var percentage = position / range;
			return percentage;
		}
	}

	public struct StructNearestNumbers
	{
		public float Lower;
		public float Upper;
		public int IndexOfLower;
	}
}