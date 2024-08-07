using System;
using System.Collections;
using System.Collections.Generic;
using _Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ProgressBars
{
	public class ProgressBar : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Image _fill;
		[SerializeField] private RectTransform _icon;
		[SerializeField] private RectTransform _iconBoss;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TextMeshProUGUI _progress;

		[Header("Settings")]
		[SerializeField] private bool _isNotSwitch;
		[SerializeField] private bool _hideIdle;
		[SerializeField] private float _hideDelay;
		[SerializeField] private bool _hideEmpty;
		[SerializeField] private float _switchDuration = 0.2f;
		[SerializeField] private float _fillDuration = 0.2f;
		[SerializeField] private Color _fillColor = Color.white;

		[Header("Segments")]
		[SerializeField] private bool _enableSegments;
		[SerializeField] private List<Image> _imageSegments;

		private float _maxValue = 1f;
		private float _currentValue;
		private Coroutine _showCor;
		private Tweener _fillTweener;

		private float _halfWidth;
		private float _minIconX;
		private float _maxIconX;
		private bool _isShowCanvas;
		private List<float> _segments;

		public float Percent => _fill.fillAmount;
		public float CurrentValue => _currentValue;
		public float MaxValue => _maxValue;

		public void SetSegments(float oneSegmentPercent, float twoSegmentPercent, float threeSegmentPercent)
			=> _segments = new List<float> {oneSegmentPercent, twoSegmentPercent, threeSegmentPercent, 1f};

		public void Show(float duration)
		{
			if (_isShowCanvas)
				return;

			_isShowCanvas = true;
			_canvasGroup.DOFade(1, duration).SetLink(gameObject).SetUpdate(true);
		}

		public void Hide(float duration)
		{
			_isShowCanvas = false;
			_canvasGroup.DOFade(0, duration).SetLink(gameObject).SetUpdate(true);
		}

		public void ResetSegments()
		{
			if (_enableSegments)
				_imageSegments.ForEach(x => x.fillAmount = 0f);
		}

		private void Awake()
		{
			//SetColor();
			_fill.fillAmount = 0;
			if (_progress != default)
				_progress.text = "";

			InitWidth();
		}

		private void LateUpdate()
		{
			if (_icon == default)
				return;

			var xPos = Mathf.Lerp(-_halfWidth, _halfWidth, _fill.fillAmount);
			_icon.anchoredPosition = new Vector2(xPos, _icon.anchoredPosition.y);
		}

		public void SetBossIcon(float currentBoss)
		{
			if (_iconBoss == default)
				return;

			var xPos = Mathf.Lerp(-_halfWidth, _halfWidth, currentBoss);
			_iconBoss.anchoredPosition = Vector2.right * xPos;
		}

		private void InitWidth()
		{
			_halfWidth = _fill.rectTransform.rect.width / 2;
		}

		public void Switch(bool enable)
		{
			if (_isNotSwitch)
				return;

			if (enable)
				Show(_switchDuration);
			else
				Hide(_switchDuration);
		}

		public void SetMaxValue(float maxValue, bool force)
		{
			_maxValue = maxValue;

			if (_enableSegments)
				UpdateSegmentsBar(force);
			else
				UpdateBar(force);
		}

		public void SetValue(float value, bool force = false, bool useFakePercent = false)
		{
			_currentValue = value;

			if (_enableSegments)
				UpdateSegmentsBar(force, useFakePercent);
			else
				UpdateBar(force, useFakePercent);
		}

		public void DoFill(float duration, bool autoHide = false, Action onComplete = null)
		{
			SetMaxValue(duration, true);
			Switch(true);
			_fillTweener = DOTween.To(x => SetValue(x, true), 0, duration, duration)
			                      .SetLink(gameObject)
			                      .OnComplete(() =>
			                      {
				                      if (autoHide)
					                      Switch(false);
				                      onComplete?.Invoke();
			                      });
		}

		private void UpdateBar(bool force = false, bool useFakePercent = false)
		{
			var percent = _currentValue / _maxValue;
			var fakePercent = _currentValue > 0 ? Mathf.Lerp(0.055f, 1, percent) : percent;
			_fill.DOKill();

			if (force)
				_fill.fillAmount = useFakePercent ? fakePercent : percent;
			else
				_fill.DOFillAmount(useFakePercent ? fakePercent : percent, _fillDuration).SetLink(gameObject);

			if (_progress != default)
				_progress.text = percent <= 0 ? "" : $"{(int) (percent * 100)}%";

			if (!_hideIdle && _hideEmpty)
				Switch(percent > 0);
			if (!_hideIdle)
				return;

			ShowAndHide();
		}

		private void UpdateSegmentsBar(bool force = false, bool useFakePercent = false)
		{
			if (_segments == default)
				return;

			var percent = _currentValue / _maxValue;
			var fakePercent = _currentValue > 0 ? Mathf.Lerp(0.055f, 1, percent) : percent;
			_fill.DOKill();

			var currentPercent = useFakePercent ? fakePercent : percent;
			ref var structNearest = ref Helper.NearestNumbers(_segments, currentPercent);

			for (var i = 0; i < _imageSegments.Count; i++)
			{
				if (structNearest.IndexOfLower == i)
					_imageSegments[structNearest.IndexOfLower].fillAmount
						= Helper.SegmentReverseLerp(structNearest, currentPercent);
				else if (structNearest.IndexOfLower < i)
					_imageSegments[i].fillAmount = default;
				else
					_imageSegments[i].fillAmount = 1f;
			}

			if (_progress != default)
				_progress.text = percent <= 0 ? "" : $"{(int) (percent * 100)}%";

			if (!_hideIdle && _hideEmpty)
				Switch(percent > 0);
			if (!_hideIdle)
				return;

			ShowAndHide();
		}

		public void SetColor(Color color = default)
		{
			_fill.color = color != default ? color : _fillColor;
		}

		public void FlashColor(Color colorSwap, float flexColorDuration, int countLoop)
		{
			_fill.DOKill();
			_fill.DOColor(colorSwap, flexColorDuration).SetLink(gameObject).SetLoops(countLoop, LoopType.Yoyo);
		}

		private void ShowAndHide()
		{
			Show(_switchDuration);
			_showCor.Stop(this);
			_showCor = StartCoroutine(ShowCor());
		}

		private IEnumerator ShowCor()
		{
			yield return new WaitForSeconds(_hideDelay);

			Hide(_switchDuration);
		}

		private void OnDestroy()
		{
			_fillTweener.Kill();
			_showCor.Stop(this);
		}
	}
}