using UnityEngine;
using static Common.Constants.SavableValuePrefix;

namespace Common
{
	public class FloatDataValueSavable : DataValueSavable<float>
	{
		public FloatDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetFloat(FLOAT_DATA_VALUE + SaveKey, 0f);
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetFloat(FLOAT_DATA_VALUE + SaveKey, Value);
		}

		public override void Delete()
		{
			base.Delete();
			PlayerPrefs.DeleteKey(FLOAT_DATA_VALUE + SaveKey);
		}

		public override bool HasSaving()
		{
			return PlayerPrefs.HasKey(FLOAT_DATA_VALUE + SaveKey);
		}
	}
}