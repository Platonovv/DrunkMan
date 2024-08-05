using UnityEditor;
using UnityEngine;

public static class PlayerPrefsTool
{
	[MenuItem("PlayerPrefs/Clear All")]
	public static void ClearAllPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}