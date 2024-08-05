using UnityEngine;

namespace _Game.DrunkManSpawner.Data
{
	[CreateAssetMenu(fileName = "DrunkMan", menuName = "Level/DrunkMan")]
	public class DrunkManData : ScriptableObject
	{
		[SerializeField] private DrunkManType _drunkManType;

		public DrunkManType DrunkManType => _drunkManType;
	}

	public enum DrunkManType
	{
		Noting = 0,
		Normal = 1,
	}
}