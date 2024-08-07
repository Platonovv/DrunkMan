using System;
using System.Collections.Generic;
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
		[SerializeField] private WinCircle _winCircle;
		

		private BaseMixerUI _currentMixer;
		private DrunkManFactory _drunkFactory;
		private Inventory _currentInventory;
		private CharacterBase _currentDrunkMan;

		public void Init(BaseMixerUI currentMixer, Inventory currentInventory)
		{
			_currentMixer = currentMixer;
			_currentInventory = currentInventory;

			Subscribe();
		}

		private void OnDestroy()
		{
			UnSubscribe();
		}

		private void Subscribe()
		{
			_currentMixer.OnStartSpawn += StarLevel;
			_currentMixer.OnStartMix += StartMixer;
			_currentMixer.OnStartMove += StarMove;
			_currentMixer.OnStartAgent += StartAgent;
			_currentInventory.SlotDraggedView.OnShowVisualPath += ShowShowVisualDrawPath;
			_currentInventory.SlotDraggedView.OnHideVisualPath += HideVisualDrawPath;

			_drunkManHandler.OnSpawnDrunkMan += SpawnDrunkMan;

			_winCircle.OnWinLevel += Win;
		}

		private void UnSubscribe()
		{
			_currentMixer.OnStartSpawn -= StarLevel;
			_currentMixer.OnStartMix -= StartMixer;
			_currentMixer.OnStartMove -= StarMove;
			_currentMixer.OnStartAgent -= StartAgent;
			_currentInventory.SlotDraggedView.OnShowVisualPath -= ShowShowVisualDrawPath;
			_currentInventory.SlotDraggedView.OnHideVisualPath -= HideVisualDrawPath;

			_drunkManHandler.OnSpawnDrunkMan -= SpawnDrunkMan;
			
			_winCircle.OnWinLevel -= Win;
		}

		private void Win() => OnWinLevel?.Invoke();
		private void Lose() => OnLoseLevel?.Invoke();

		private void StarLevel()
		{
			_currentInventory.Init();
			_currentInventory.ShowInventory(true);
			_drunkManHandler.StartSpawn(_drunkManData);
		}

		private void ShowShowVisualDrawPath(BarIngredient barIngredient)
			=> _currentDrunkMan.SetLineRenderer(barIngredient.WayPoints);

		private void StartAgent(BarIngredient barIngredient) 
			=> _currentDrunkMan.MoveAgent(barIngredient.WayPoints);

		private void HideVisualDrawPath(BarIngredient barIngredient)
			=> _currentDrunkMan.ClearLastPath(barIngredient.WayPoints);

		private void StarMove() => _currentDrunkMan.PlayAgent();

		private void SpawnDrunkMan(CharacterBase characterBase)
		{
			if (_currentDrunkMan != default)
			{
				_currentDrunkMan.OnEndPath -= EndMix;
				_currentDrunkMan.OnDeath -= Lose;
			}

			_currentDrunkMan = characterBase;
			_currentDrunkMan.OnEndPath += EndMix;
			_currentDrunkMan.OnDeath += Lose;
			FollowCamera(characterBase.transform);
		}

		private void StartMixer()
		{
			_currentInventory.ShowInventory(false);
		}

		private void EndMix()
		{
			_currentMixer.EndMix();
			_currentInventory.ShowInventory(true);
		}

		private void FollowCamera(Transform followTransform)
		{
			_followCamera.Follow = followTransform;
			_followCamera.LookAt = followTransform;
		}
	}
}