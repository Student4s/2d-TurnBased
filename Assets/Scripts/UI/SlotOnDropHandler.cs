using UnityEngine;
using UnityEngine.EventSystems;

public class SlotOnDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped on slot");

        // GameObject droppedObject = eventData.pointerDrag;

        // if (droppedObject != null)
        // {
        //     ItemUI droppedItemUI = droppedObject.GetComponent<ItemUI>();
            
        //     if (TryGetComponent<SlotUI>(out var slot))
        //     {
        //         if (droppedItemUI != null)
        //         {
        //             slot.PlaceItem(droppedItemUI);
        //         }

        //     }
        //     else
        //     {
        //         Debug.Log("Does not contain SlotUI");
        //         //ResetItemPosition(droppedItemUI);
        //     }
            
        // }
    }

    private void ResetItemPosition(ItemUI itemUI)
    {
        // RectTransform rectTransform = itemUI.GetComponent<RectTransform>();
        // rectTransform.SetParent(itemUI.transform.parent, true);
        // Vector2 gridPosition = new Vector2(itemUI.GridPosition.x * InventoryUI.CellSize, -itemUI.GridPosition.y * InventoryUI.CellSize);
        // rectTransform.anchoredPosition = gridPosition;
    }
}
