using DG.Tweening;
using UnityEngine;

namespace _Tools.FlashingMesh
{
	public class Flasher : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

		[SerializeField] private string _emissionProperty = "_EmissionColor";
		[SerializeField] private Color _flashColor;
		[SerializeField] private float _flashDuration;

		private bool _isFlashing;

		public void DoFlash()
		{
			if (_isFlashing)
				return;

			if (_meshRenderer != default)
				Flash(_meshRenderer.material);
			if (_skinnedMeshRenderer != default)
				Flash(_skinnedMeshRenderer.material);
		}

		private void Flash(Material material)
		{
			_isFlashing = true;
			material.DOKill();
			material.DOColor(_flashColor, _emissionProperty, _flashDuration)
			        .SetLink(gameObject)
			        .OnComplete(() =>
			        {
				        material.DOColor(Color.clear, _emissionProperty, _flashDuration)
				                .SetLink(gameObject)
				                .OnComplete(() => _isFlashing = false);
			        });
		}
	}
}