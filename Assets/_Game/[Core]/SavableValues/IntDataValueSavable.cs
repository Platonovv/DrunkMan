using UnityEngine;
using static Common.Constants.SavableValuePrefix;

namespace Common
{
	public class IntDataValueSavable : DataValueSavable<int>
	{
		public IntDataValueSavable(string saveKey) : base(saveKey) { }

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetInt(INT_DATA_VALUE + SaveKey, 0);
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetInt(INT_DATA_VALUE + SaveKey, Value);
		}

		public override void Delete()
		{
			base.Delete();
			PlayerPrefs.DeleteKey(INT_DATA_VALUE + SaveKey);
		}

		public override bool HasSaving()
		{
			return PlayerPrefs.HasKey(INT_DATA_VALUE + SaveKey);
		}
	}
}