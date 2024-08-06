using System;
using System.Collections.Generic;
using _Game.BarCatalog;
using _Game.BarInventory;
using _Game.DrunkManSpawner;
using _Game.DrunkManSpawner.Data;
using _Game.Mixer;
using Cinemachine;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class Level : MonoBehaviour
	{
		public event Action OnWinLevel;
		public event Action OnLoseLevel;
		
		[SerializeField] private List<DrunkManData> _drunkManData;
		[SerializeField] private CinemachineVirtualCamera _followCamera;
		[SerializeField] private DrunkManHandler _drunkManHandler;

		private BaseMixerUI _currentMixer;
		private DrunkManFactory _drunkFactory;
		private Inventory _currentInventory;

		public void Init(BaseMixerUI currentMixer, Inventory currentInventory)
		{
			_currentMixer = currentMixer;
			_currentInventory = currentInventory;
			_currentMixer.OnStartMixer += StarLevel;
			_drunkManHandler.OnSpawnDrunkMan += FollowDrunkMan;
		}

		private void OnDestroy()
		{
			_currentMixer.OnStartMixer -= StarLevel;
			_drunkManHandler.OnSpawnDrunkMan -= FollowDrunkMan;
		}

		private void StarLevel()
		{
			_currentInventory.Init();
			_drunkManHandler.StartSpawn(_drunkManData);
		}

		private void FollowDrunkMan(Transform followTransform)
		{
			_followCamera.Follow = followTransform;
			_followCamera.LookAt = followTransform;
		}
	}
}