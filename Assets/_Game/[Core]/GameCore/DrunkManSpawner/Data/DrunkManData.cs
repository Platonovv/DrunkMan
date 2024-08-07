﻿using Gameplay.Characters;
using UnityEngine;

namespace _Game.DrunkManSpawner.Data
{
	[CreateAssetMenu(fileName = "DrunkMan", menuName = "Level/DrunkMan")]
	public class DrunkManData : ScriptableObject
	{
		[SerializeField] private int _health = 100;
		[SerializeField] private float _speed = 1f;
		[SerializeField] private DrunkManType _drunkManType;
		[SerializeField] private CharacterBase _characterPrefab;

		public DrunkManType DrunkManType => _drunkManType;
		public CharacterBase CharacterPrefab => _characterPrefab;
		public int Health => _health;
		public float Speed => _speed;
	}

	public enum DrunkManType
	{
		Noting = 0,
		Slow = 1,
		Normal = 2,
		Fast = 3,
	}
}