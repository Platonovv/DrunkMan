using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MainMenu.GangPage
{
	public abstract class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		protected IDragCardIntermediary DragCardIntermediary;

		public void Init(IDragCardIntermediary dragCardIntermediary) => DragCardIntermediary = dragCardIntermediary;

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (DragCardIntermediary == default)
				return;
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (DragCardIntermediary == default)
				return;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (DragCardIntermediary == default)
				return;
		}

		protected virtual void ReturnItem()
		{
		}
	}
}