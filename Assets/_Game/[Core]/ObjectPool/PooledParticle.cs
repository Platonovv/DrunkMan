using UnityEngine;

namespace Utils.ObjectPool
{
	[RequireComponent(typeof(ParticleSystem))]
	[RequireComponent(typeof(ReleaseAfterDelay))]
	public class PooledParticle : PoolItem
	{
		[SerializeField] private ParticleSystem    _particle;
		[SerializeField] private ReleaseAfterDelay _releaseAfterDelay;
		[SerializeField] private bool              _playOnAwake;

		public override void Restart()
		{
			base.Restart();
			if (_playOnAwake) _particle.Play();
		}

		public override void Retain(int id, string containerName)
		{
			base.Retain(id, containerName);
			if (_playOnAwake) _particle.Play();
		}

		public override void Release(bool disableObject = true)
		{
			_particle.Stop();
			base.Release(disableObject);
		}

		public void StopRelease() => _releaseAfterDelay.enabled = false;

		public void ReleaseAfter(float delay = 0)
		{
			_releaseAfterDelay.SetDelay(delay);
			_releaseAfterDelay.enabled = true;
		}

		public void Play() => _particle.Play();
		public void Stop() => _particle.Stop();
		public void Pause() => _particle.Pause();
	}
}