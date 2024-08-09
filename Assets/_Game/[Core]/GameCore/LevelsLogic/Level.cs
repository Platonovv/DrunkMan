using System;
using System.Collections.Generic;
using System.Linq;
using _Game.BarCatalog;
using _Game.BarInventory;
using _Game.DrunkManSpawner;
using _Game.Mixer;
using _Game.UI.Quests;
using _Tools;
using Cinemachine;
using GameManager.LevelsLogic.Data;
using Gameplay.Characters;
using UI;
using UnityEngine;

namespace GameManager.LevelsLogic
{
	public class Level : MonoBehaviour
	{
		public event Action OnWinLevel;
		public event Action OnLoseLevel;

		[SerializeField] private CinemachineVirtualCamera _followCamera;
		[SerializeField] private MiniMapCameraHelper _miniMapCameraHelper;
		[SerializeField] private SpawnCharacterHandler _spawnCharacterHandler;
		[SerializeField] private QuestCircleHandler _questCircleHandler;

		private BaseMixerUI _currentMixer;
		private CharacterFactory _characterFactory;
		private Inventory _currentInventory;
		private CharacterBase _currentDrunkMan;
		private QuestView _mainGUIQuestView;
		private List<CharacterBase> _nps;
		public CinemachineVirtualCamera FollowCamera1 => _followCamera;

		public void Init(BaseMixerUI currentMixer, Inventory currentInventory, QuestView mainGUIQuestView)
		{
			_currentMixer = currentMixer;
			_currentInventory = currentInventory;
			_mainGUIQuestView = mainGUIQuestView;

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

			_spawnCharacterHandler.OnSpawnDrunkMan += SpawnSpawnCharacter;
			_spawnCharacterHandler.OnSpawnNPS += SpawnNps;

			_questCircleHandler.OnWinLevel += CompleteQuest;
			_questCircleHandler.OnSpawnQuestCircle += SetQuest;
			_questCircleHandler.OnSpawnAdditionalQuestCircle += SetAdditionalQuest;
		}

		private void UnSubscribe()
		{
			_currentMixer.OnStartSpawn -= StarLevel;
			_currentMixer.OnStartMix -= StartMixer;
			_currentMixer.OnStartMove -= StarMove;
			_currentMixer.OnStartAgent -= StartAgent;
			_currentInventory.SlotDraggedView.OnShowVisualPath -= ShowShowVisualDrawPath;
			_currentInventory.SlotDraggedView.OnHideVisualPath -= HideVisualDrawPath;

			_spawnCharacterHandler.OnSpawnDrunkMan -= SpawnSpawnCharacter;
			_spawnCharacterHandler.OnSpawnNPS -= SpawnNps;

			_questCircleHandler.OnWinLevel -= CompleteQuest;
			_questCircleHandler.OnSpawnQuestCircle -= SetQuest;
			_questCircleHandler.OnSpawnAdditionalQuestCircle -= SetAdditionalQuest;
		}

		private void CompleteQuest()
		{
			ResourceHandler.AddResource(ResourceType.Money, (int) _currentDrunkMan.Health);
			OnWinLevel?.Invoke();
		}

		private void Lose() => OnLoseLevel?.Invoke();

		private void StarLevel()
		{
			_currentInventory.Init();
			_currentInventory.ShowInventory(true);
			_spawnCharacterHandler.SpawnPlayer();
			_questCircleHandler.StartRandomQuest();
			_questCircleHandler.StartRandomAdditionalQuest();
		}

		private void ShowShowVisualDrawPath(BarIngredient barIngredient)
			=> _currentDrunkMan.SetLineRenderer(barIngredient.WayPoints);

		private void StartAgent() => _currentDrunkMan.MoveAgent();

		private void HideVisualDrawPath(BarIngredient barIngredient)
			=> _currentDrunkMan.ClearLastPath(barIngredient.WayPoints);

		private void StarMove() => _currentDrunkMan.PlayAgent();

		private void SetQuest(QuestData questData)
		{
			_mainGUIQuestView.SetQuestImage(questData.QuestSprite);
			UpdateQuestReward(questData.QuestReward);
			_currentDrunkMan.InitHealForRewardQuest(questData.QuestReward);
		}

		private void SetAdditionalQuest(QuestData questData)
		{
			var nps = _nps.FirstOrDefault();

			if (nps != default)
				nps.SetQuest(questData);
		}

		private void SpawnNps(List<CharacterBase> nps)
		{
			_nps = nps;
		}

		private void UpdateQuestReward(float value) => _mainGUIQuestView.SetQuestReward(value);

		private void SpawnSpawnCharacter(CharacterBase characterBase)
		{
			if (_currentDrunkMan != default)
			{
				_currentDrunkMan.OnEndPath -= EndMix;
				_currentDrunkMan.OnDeath -= Lose;
				_currentDrunkMan.OnTakeDamage -= UpdateQuestReward;
			}

			_currentDrunkMan = characterBase;
			_currentDrunkMan.OnEndPath += EndMix;
			_currentDrunkMan.OnDeath += Lose;
			_currentDrunkMan.OnTakeDamage += UpdateQuestReward;
			//FollowCamera(characterBase.transform);
			FollowMiniMap(characterBase.transform);
		}

		private void FollowMiniMap(Transform characterBaseTransform)
		{
			_miniMapCameraHelper.SetFollow(characterBaseTransform);
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