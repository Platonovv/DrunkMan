using System;
using System.Collections.Generic;
using System.Linq;
using _Game.BarCatalog;
using _Game.BarInventory;
using _Game.DrunkManSpawner;
using _Game.DrunkManSpawner.Data;
using _Game.Mixer;
using Cinemachine;
using Gameplay.Characters;
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
		private CharacterBase _currentDrunkMan;

		public void Init(BaseMixerUI currentMixer, Inventory currentInventory)
		{
			_currentMixer = currentMixer;
			_currentInventory = currentInventory;
			_currentMixer.OnStartMixer += StarLevel;
			_currentMixer.OnStartMove += StarMove;
			_currentMixer.OnDrawPath += StartDrawPath;
			_drunkManHandler.OnSpawnDrunkMan += SpawnDrunkMan;
		}

		private void OnDestroy()
		{
			_currentMixer.OnStartMixer -= StarLevel;
			_currentMixer.OnStartMove -= StarMove;
			_currentMixer.OnDrawPath -= StartDrawPath;
			_drunkManHandler.OnSpawnDrunkMan -= SpawnDrunkMan;
		}

		private void StarLevel()
		{
			_currentInventory.Init();
			_drunkManHandler.StartSpawn(_drunkManData);
		}

		private void StartDrawPath(BarIngredient barIngredient)
		{
			_currentDrunkMan.SetLineRenderer(barIngredient.WayPoints);
			_currentDrunkMan.MoveAgent(barIngredient.WayPoints);
			_currentDrunkMan.PlayAgent();
		}

		private void StartDrawPath(List<BarIngredient> barIngredients)
		{
			foreach (var barIngredient in barIngredients)
				_currentDrunkMan.SetLineRenderer(barIngredient.WayPoints);
		}

		private void StarMove()
		{
			_currentDrunkMan.PlayAgent();
		}

		private void SpawnDrunkMan(CharacterBase characterBase)
		{
			_currentDrunkMan = characterBase;
			FollowCamera(characterBase.transform);
		}

		private void FollowCamera(Transform followTransform)
		{
			_followCamera.Follow = followTransform;
			_followCamera.LookAt = followTransform;
		}
	}
}