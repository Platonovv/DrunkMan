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
using Gameplay.Characters.Enemies;
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
		private List<CharacterBase> _enemies;
		private QuestData _questAdditionalData;
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

			if (_enemies == default)
				return;

			foreach (var characterBase in _enemies)
				if (characterBase is Enemy enemy)
					enemy.QuestComplete.OnTargetQuestComplete -= CompleteTargetQuest;
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
			_spawnCharacterHandler.OnSpawnNps += SpawnNps;
			_spawnCharacterHandler.OnSpawnEnemy += SpawnEnemy;

			_questCircleHandler.OnWinLevel += CompleteQuest;
			_questCircleHandler.OnSpawnQuestCircle += SetQuest;
			_questCircleHandler.OnSpawnAdditionalQuestCircle += SetAdditionalQuest;
			_questCircleHandler.OnSpawnAdditionalQuestTarget += SetAdditionalQuestTarget;
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
			_spawnCharacterHandler.OnSpawnNps -= SpawnNps;
			_spawnCharacterHandler.OnSpawnEnemy -= SpawnEnemy;

			_questCircleHandler.OnWinLevel -= CompleteQuest;
			_questCircleHandler.OnSpawnQuestCircle -= SetQuest;
			_questCircleHandler.OnSpawnAdditionalQuestCircle -= SetAdditionalQuest;
			_questCircleHandler.OnSpawnAdditionalQuestTarget -= SetAdditionalQuestTarget;
		}

		private void CompleteQuest()
		{
			ResourceHandler.AddResource(ResourceType.Money, (int) _currentDrunkMan.Health);
			OnWinLevel?.Invoke();
		}

		private void CompleteTargetQuest()
			=> ResourceHandler.AddResource(ResourceType.Money, _questAdditionalData.QuestReward);

		private void Lose() => OnLoseLevel?.Invoke();

		private void StarLevel()
		{
			_currentInventory.Init();
			_currentInventory.ShowInventory(true);
			_spawnCharacterHandler.SpawnPlayer();
			_questCircleHandler.StartRandomQuest();
			_questCircleHandler.StartRandomAdditionalQuestTarget();
		}

		private void ShowShowVisualDrawPath(BarIngredient barIngredient)
		{
			if (_currentDrunkMan != default)
				_currentDrunkMan.SetLineRenderer(barIngredient.WayPoints);
		}

		private void StartAgent()
		{
			if (_currentDrunkMan != default)
				_currentDrunkMan.MoveAgent();
		}

		private void HideVisualDrawPath(BarIngredient barIngredient)
		{
			if (_currentDrunkMan != default)
				_currentDrunkMan.ClearLastPath(barIngredient.WayPoints);
		}

		private void StarMove()
		{
			if (_currentDrunkMan != default)
				_currentDrunkMan.PlayAgent();
		}

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

		private void SetAdditionalQuestTarget(QuestData questData)
		{
			_questAdditionalData = questData;
			List<IQuestTarget> questTargetList = new();

			foreach (var characterBase in _enemies)
				if (characterBase is IQuestTarget questTarget)
					questTargetList.Add(questTarget);

			questTargetList.ForEach(x => x.ActivateQuest(false));
			var targetQuest = questTargetList.FirstOrDefault(x => x.EnemyType == questData.EnemyType);
			targetQuest?.ActivateQuest(true);

			var nps = _nps.FirstOrDefault();
			if (nps != default)
				nps.SetQuest(questData);
		}

		private void SpawnNps(List<CharacterBase> nps)
		{
			_nps = nps;
		}

		private void SpawnEnemy(List<CharacterBase> enemies)
		{
			_enemies = enemies;

			foreach (var characterBase in _enemies)
				if (characterBase is Enemy enemy)
					enemy.QuestComplete.OnTargetQuestComplete += CompleteTargetQuest;
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