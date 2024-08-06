using _Game.BarInventory;
using UnityEngine.EventSystems;

namespace UI.MainMenu.GangPage.Dragged
{
	public class BarIngredientCollectionDragHandler : DragHandler
	{
		private IngredientDraggedView _ingredientDraggedView;

		public void InitWeaponCardDraggedView(IngredientDraggedView ingredientDraggedView)
			=> _ingredientDraggedView = ingredientDraggedView;

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);

			if (DragCardIntermediary.DraggedState != DraggedState.CanTouch)
				return;

			_ingredientDraggedView.SetDraggedView(DragCardIntermediary.BarIngredient);
			_ingredientDraggedView.StartDrag(DragCardIntermediary.Transform);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);

			if (DragCardIntermediary.DraggedState != DraggedState.CanTouch)
				return;

			_ingredientDraggedView.Move(eventData.delta);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);

			if (DragCardIntermediary.DraggedState != DraggedState.CanTouch)
				return;

			if (eventData == default)
				return;

			if (eventData.pointerEnter != default
			    && eventData.pointerEnter.TryGetComponent(out IDragSlotCollectionReceiver dragReceiver))
			{
				dragReceiver.AddInCollection(DragCardIntermediary.BarIngredient);
			}
			else
			{
				//ReturnItem();
			}

			_ingredientDraggedView.EndDrag();
		}

		private void Awake()
		{
			var intermediary = GetComponent<SlotInventory>();

			if (intermediary == default)
				return;

			DragCardIntermediary = intermediary;
		}

		protected override void ReturnItem() => DragCardIntermediary.ReturnItem();
	}
}