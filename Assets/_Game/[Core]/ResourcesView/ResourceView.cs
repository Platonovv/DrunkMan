using System;
using System.Collections;
using System.Collections.Generic;
using _Tools;
using DG.Tweening;
using ScriptableObjects.Classes.Resources;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.ResourcesView
{
	public class ResourceView : MonoBehaviour
	{
		[SerializeField] private Transform _resourceViewElementParent;
		[SerializeField] private Transform _resourceParent;
		[SerializeField] private CanvasGroup _myCanvasGroup;

		[SerializeField] private ResourceViewElement _resourceViewElementPrefab;

		[SerializeField] private bool _autoHide;
		[SerializeField] private bool _shrink;
		[SerializeField] private bool _resourceFly;
		[SerializeField] private int _maxSpawnCount = 15;
		[SerializeField] private int _minAddedValueAtOneTime = 10;
		[SerializeField] private float _randomRadius = 100;
		[SerializeField] private float _flyDuration = 0.6f;

		[SerializeField] private Image _resourceViewPrefab;

		[SerializeField] protected List<ResourceData> _showResourceData;

		private readonly Dictionary<ResourceData, ResourceViewElement> _resourceViewElements = new();

		private readonly Stack<Image> _flyResources = new Stack<Image>();

		private Sequence _flySequence;

		private void OnEnable()
		{
			ResourceHandler.OnValueAdded += AddResourceCount;
			ResourceHandler.OnValueSet += SetResourceCount;
			ResourceHandler.OnValueSubtracted += SubtractResource;
		}

		private void OnDisable()
		{
			ResourceHandler.OnValueAdded -= AddResourceCount;
			ResourceHandler.OnValueSet -= SetResourceCount;
			ResourceHandler.OnValueSubtracted -= SubtractResource;
		}

		private void Awake() => ClearElements();
		private void Start() => ResourceHandler.LoadAllData();

		private void ClearElements()
		{
			_resourceViewElementParent.DestroyChildren();
			_resourceViewElements.Clear();
		}

#if UNITY_EDITOR
		private void UpdateViews()
		{
			if (Application.isPlaying)
			{
				ClearElements();
				ResourceHandler.LoadAllData();
			}
			else
			{
				try
				{
					ClearElements();
					foreach (var resourceData in _showResourceData)
					{
						SetResourceCount(resourceData.Type, 100);
					}

					ResourceHandler.LoadAllData();
				}
				catch (Exception e)
				{
					Debug.LogWarning("Not enough permissions, try editing prefab");
				}
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				ResourceHandler.AddResource(ResourceType.Money, 100);
			}

			if (Input.GetKeyDown(KeyCode.M))
			{
				ResourceHandler.TrySubtractResource(ResourceType.Money, 100);
			}
		}
#endif

		private void AddResourceCount(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element))
			{
				AddResourceCount(element, value);
			}
		}

		private void SubtractResource(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element))
			{
				element.SubtractResource(value);
			}
		}

		private void SetResourceCount(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element))
			{
				element.SetResourceCount(value);
			}
		}

		private bool TryGetElement(ResourceType type, out ResourceViewElement element)
		{
			var resourceData = GetResourceData(type);

			if (resourceData != default)
			{
				if (_resourceViewElements.TryGetValue(resourceData, out var viewElement))
				{
					element = viewElement;
				}
				else
				{
					element = Instantiate(_resourceViewElementPrefab, _resourceViewElementParent);
					element.Init(resourceData, _autoHide, _shrink);
					element.name = $"{type.ToString()} View Element";
					_resourceViewElements.Add(resourceData, element);
				}

				return true;
			}

			element = null;
			return false;
		}

		private ResourceData GetResourceData(ResourceType type)
		{
			return _showResourceData.Find(x => x.Type == type);
		}

		private void AddResourceCount(ResourceViewElement viewElement, int value)
		{
			StartCoroutine(AddResourceCountCor(viewElement, value));
		}

		private IEnumerator AddResourceCountCor(ResourceViewElement viewElement, int value)
		{
			viewElement.SetCanvasShow(true);
			yield return null;

			if (!_resourceFly)
			{
				viewElement.AddResource(value);
				yield break;
			}

			var resourceCount = Mathf.CeilToInt((float) value / _minAddedValueAtOneTime);

			if (resourceCount > _maxSpawnCount)
				resourceCount = _maxSpawnCount;

			var defaultIncreaseValue = Mathf.FloorToInt((float) value / resourceCount);
			var lastIncreaseValue = value - (defaultIncreaseValue * resourceCount) + defaultIncreaseValue;

			var globalFlySequence = DOTween.Sequence();

			var flyResourceTemp = new List<Image>();

			for (var i = 0; i < resourceCount; i++)
			{
				var index = i;
				var flyResource = GetFlyResource(GetResourceData(viewElement.Type));

				var flyTransform = flyResource.transform;
				flyTransform.DOKill();
				var flyResourcePosition = flyTransform.position;

				flyResourceTemp.Add(flyResource);

				flyTransform.localScale = Vector3.zero;
				flyTransform.localPosition = Random.insideUnitCircle * _randomRadius;
				//flyTransform.SetParent(viewElement.ResourceIconTransform);
				flyResource.gameObject.SetActive(true);
				_flySequence?.Kill();
				_flySequence = DOTween.Sequence();

				var delay = i == 0 ? 0 : Random.Range(0.1f, 0.25f);
				_flySequence.Append(flyTransform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc).SetDelay(delay));
				_flySequence.Join(flyTransform.DOMove(viewElement.ResourceIconPosition, _flyDuration)
				                              .SetEase(Ease.InBack));
				_flySequence.OnComplete(delegate
				{
					flyTransform.DOScale(0f, 0.2f)
					            .SetEase(Ease.InCirc)
					            .OnComplete(delegate
					            {
						            flyResource.gameObject.SetActive(false);
						            flyTransform.SetParent(_resourceParent);
					            });

					viewElement.AddResource(index == resourceCount - 1 ? lastIncreaseValue : defaultIncreaseValue);
					//MyVibration.Haptic(MyHapticTypes.LightImpact);
				});

				globalFlySequence.Join(_flySequence);
				globalFlySequence.OnComplete(delegate
				{
					foreach (var resource in flyResourceTemp)
					{
						resource.gameObject.SetActive(false);
						resource.transform.localPosition = Vector3.zero;
						_flyResources.Push(resource);
					}
				});
			}
		}

		private void SpawnResource()
		{
			var resource = Instantiate(_resourceViewPrefab, _resourceParent);
			resource.gameObject.SetActive(false);
			resource.transform.localPosition = Vector3.zero;
			_flyResources.Push(resource);
		}

		private Image GetFlyResource(ResourceData resourceData)
		{
			if (_flyResources.Count == 0)
				SpawnResource();
			var resource = _flyResources.Pop();
			resource.sprite = resourceData.ResourceIcon;
			return resource;
		}
	}
}