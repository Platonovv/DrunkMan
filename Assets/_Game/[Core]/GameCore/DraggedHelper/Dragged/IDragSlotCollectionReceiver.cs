using _Game.BarCatalog;
using UI.MainMenu.GangPage;

namespace UI.MainMenu
{
	public interface IDragSlotCollectionReceiver : IDragReceiver
	{
		public void AddInCollection(BarIngredient slotData);
	}
}