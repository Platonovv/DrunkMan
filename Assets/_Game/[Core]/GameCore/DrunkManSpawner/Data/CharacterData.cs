using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner.Data
{
	[CreateAssetMenu(fileName = "CharacterData", menuName = "Level/CharacterData")]
	public class CharacterData : ScriptableObject
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private DrunkManType _drunkManType;
		[SerializeField] private CharacterBase _characterPrefab;

		public DrunkManType DrunkManType => _drunkManType;
		public CharacterBase CharacterPrefab => _characterPrefab;
		public float Speed => _speed;
	}

	public enum DrunkManType
	{
		Noting = 0,
		Player = 1,
		Enemy = 2,
		NPS = 3,
	}
}