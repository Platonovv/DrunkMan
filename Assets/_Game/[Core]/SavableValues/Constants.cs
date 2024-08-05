using UnityEngine;

namespace Common
{
	public static class Constants
	{
		public static class PlayerPrefsKeyNames
		{
			public const string PLAYER_LEVEL = "Level";
			public const string CURRENT_LEVEL_NUMBER = "CurrentLevelNumber";
			public const string LAST_LEVEL_NUMBER = "LastCompletedLevelNumber";
			public const string CURRENT_DAY = "CurrentDay";
			public const string PLAYER_DAY = "PlayerDay";
			public const string WARNING_LEVEL = "WarningLevel";
			public const string PLAYER_LEVEL_EXPERIENCE = "PlayerLevelExperience";
			public const string ALL_LEVELS_COMPLETED = "AllLevelsCompleted";
			public const string PLAYER_MAX_HEAP_SIZE = "PLAYER_MAX_HEAP_SIZE";
			public const string PLAYER_SPEED = "PlayerSpeed";
			public const string START_MONEY_GET = "StartMoneyGet";
			public const string TUTORIAL_COMPLETED = "TutorialCompleted";
			public const string TUTORIAL_SECOND_COMPLETED = "TutorialSecondCompleted";
			public const string CURRENT_TUTORIAL_GROUP = "CurrentTutorialGroup";
		}

		public static class SavePrefix
		{
			public const string LEVEL = "Level_";
			public const string HEAP = "Heap_";
			public const string AREA = "Area_";
			public const string PRISONERS_COUNT = "PrisonersCount_";
			public const string BOSS_COUNT = "BossCount_";
			public const string UPGRADE = "Upgrade_";
			public const string CELL_UPGRADE = "CellUpgrade_";
			public const string WEAPON_UPGRADE = "WeaponUpgrade_";
			public const string CURRENT_WEAPON = "CurrentWeapon_";
		}

		public static class AssetPath
		{
			public const string SO_DATA_PATH = "Game/ScriptableObjects";
		}

		public static class LayersIds
		{
			public const int NAME_OBJECT_ID = 8;
		}

		public static class SavableValuePrefix
		{
			public const string INT_DATA_VALUE = "IntDataValue ";
			public const string FLOAT_DATA_VALUE = "FloatDataValue ";
			public const string STRING_DATA_VALUE = "StringDataValue ";
			public const string BOOL_DATA_VALUE = "BoolDataValue ";
			public const string VECTOR2_INT_X_DATA_VALUE = "Vector2IntXDataValue ";
			public const string VECTOR2_INT_Y_DATA_VALUE = "Vector2IntYDataValue ";
		}

		public static class Delegates
		{
			public delegate Transform TransformDelegate();

			public delegate Vector3 Vector3Delegate();

			public delegate bool BoolDelegate();
		}
	}
}