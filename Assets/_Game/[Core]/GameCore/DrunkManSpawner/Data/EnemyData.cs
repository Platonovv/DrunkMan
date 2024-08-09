using GameManager.LevelsLogic.Data;
using UnityEngine;

namespace _Game.DrunkManSpawner.Data
{
	[CreateAssetMenu(fileName = "CharacterData", menuName = "Level/EnemyData")]
	public class EnemyData : CharacterData
	{
		[SerializeField] private EnemyType _enemyType;
		public EnemyType EnemyType => _enemyType;
	}
}