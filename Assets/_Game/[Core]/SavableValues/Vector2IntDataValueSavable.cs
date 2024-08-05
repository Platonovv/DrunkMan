using UnityEngine;
using static Common.Constants.SavableValuePrefix;

namespace Common
{
	public class Vector2IntDataValueSavable
	{
		private Vector2Int _value;
		private bool       _isLoaded;

		public Vector2Int Value
		{
			get
			{
				if (!_isLoaded) Load();

				return _value;
			}

			set => _value = value;
		}

		private readonly string _saveKey;

		public Vector2IntDataValueSavable(string saveKey)
		{
			_saveKey = saveKey;
		}

		private void Load()
		{
			_isLoaded = true;
			_value = new Vector2Int(PlayerPrefs.GetInt(VECTOR2_INT_X_DATA_VALUE + _saveKey, 0),
															PlayerPrefs.GetInt(VECTOR2_INT_Y_DATA_VALUE + _saveKey, 0));
		}

		public void Save()
		{
			_isLoaded = true;
			PlayerPrefs.SetInt(VECTOR2_INT_X_DATA_VALUE + _saveKey, _value.x);
			PlayerPrefs.SetInt(VECTOR2_INT_Y_DATA_VALUE + _saveKey, _value.y);
		}

		public void Delete()
		{
			_isLoaded = false;
			PlayerPrefs.DeleteKey(VECTOR2_INT_X_DATA_VALUE + _saveKey);
			PlayerPrefs.DeleteKey(VECTOR2_INT_Y_DATA_VALUE + _saveKey);
		}

		public bool HasSaving()
		{
			return PlayerPrefs.HasKey(VECTOR2_INT_X_DATA_VALUE + _saveKey) &&
						 PlayerPrefs.HasKey(VECTOR2_INT_Y_DATA_VALUE + _saveKey);
		}
	}
}