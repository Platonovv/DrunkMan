using System.Collections.Generic;
using _Game.DrunkManSpawner.Data;
using Gameplay.Characters;
using Object = UnityEngine.Object;

namespace _Game.DrunkManSpawner
{
	public class CharacterFactory
	{
		public List<CharacterBase> GetCharacters(List<CharacterData> charactersData)
		{
			var characterBase = new List<CharacterBase>();

			foreach (var characterData in charactersData)
			{
				switch (characterData.DrunkManType)
				{
					case DrunkManType.Noting:
						break;
					case DrunkManType.Player:
					case DrunkManType.Enemy:
					case DrunkManType.NPS:
						var character = Object.Instantiate(characterData.CharacterPrefab);
						character.InitData(characterData);
						if (!characterBase.Contains(character))
							characterBase.Add(character);
						break;
				}
			}

			return characterBase;
		}
	}
}