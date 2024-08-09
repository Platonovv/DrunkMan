using GameManager.LevelsLogic.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Characters.NPS
{
	public class BaseNps : CharacterBase
	{
		[Header("Quest")]
		[SerializeField] private Image _questImage;

		public override void SetQuest(QuestData questData) => _questImage.sprite = questData.QuestSprite;
	}
}