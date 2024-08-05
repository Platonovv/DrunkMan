using UnityEngine;
using static Common.Constants.SavableValuePrefix;

namespace Common
{
	public class StringDataValueSavable : DataValueSavable<string>
	{
		public StringDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetString(STRING_DATA_VALUE + SaveKey, "");
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetString(STRING_DATA_VALUE + SaveKey, Value);
		}

		public override void Delete()
		{
			base.Delete();
			PlayerPrefs.DeleteKey(STRING_DATA_VALUE + SaveKey);
		}

		public override bool HasSaving()
		{
			return PlayerPrefs.HasKey(STRING_DATA_VALUE + SaveKey);
		}
	}
}