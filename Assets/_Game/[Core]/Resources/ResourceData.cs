using UI;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Classes.Resources
{
	[CreateAssetMenu(fileName = "New ResourceData", menuName = "Resource Data")]
	public class ResourceData : ScriptableObject
	{
		[SerializeField] private ResourceType _type;
		[SerializeField] private int _price;
		[SerializeField, Min(1)] private int _countInPrefab = 1;

		[SerializeField] private Sprite _resourceIcon;

		[SerializeField] private Mesh _resourceMesh;

		public Mesh ResourceMesh => _resourceMesh;
		public ResourceType Type => _type;
		public Sprite ResourceIcon => _resourceIcon;
		public int Price => _price;
		public int CountInPrefab => _countInPrefab;

#if UNITY_EDITOR
		private void ApplyName()
		{
			var assetPath = AssetDatabase.GetAssetPath(this);
			AssetDatabase.RenameAsset(assetPath, _type.ToString());
			AssetDatabase.Refresh();
		}
#endif
	}
}