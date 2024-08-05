using UnityEngine;
using static Common.Constants.SavableValuePrefix;

namespace Common
{
	public class BoolDataValueSavable : DataValueSavable<bool>
	{
		public BoolDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetInt(BOOL_DATA_VALUE + SaveKey, 0) == 1;
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetInt(BOOL_DATA_VALUE + SaveKey, Value ? 1 : 0);
		}

		public override void Delete()
		{
			base.Delete();
			PlayerPrefs.DeleteKey(BOOL_DATA_VALUE + SaveKey);
		}

		public override bool HasSaving()
		{
			return PlayerPrefs.HasKey(BOOL_DATA_VALUE + SaveKey);
		}
	}
}