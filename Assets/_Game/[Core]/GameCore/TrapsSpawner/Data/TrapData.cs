using UnityEngine;

namespace _Game.TrapsSpawner.Data
{
	[CreateAssetMenu(fileName = "TrapData", menuName = "Traps/Trap", order = 0)]
	public class TrapData : ScriptableObject
	{
		[SerializeField] private Trap _trapPrefab;
		[SerializeField] private TrapType _trapType;
		[SerializeField] private float _trapDamage;

		public Trap TrapPrefab => _trapPrefab;
		public TrapType TrapType => _trapType;
		public float TrapDamage => _trapDamage;
	}
}