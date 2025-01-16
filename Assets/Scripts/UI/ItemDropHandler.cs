using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            ItemUI droppedItemUI = droppedObject.GetComponent<ItemUI>();
            SlotUI slot = GetComponent<SlotUI>();

            if(slot == null){
            Debug.Log ("Does not contain SlotUI");
            } else {
            }
            if (droppedItemUI != null)
            {
                RectTransform slotRect = GetComponent<RectTransform>();
                Vector2 localPosition = slotRect.InverseTransformPoint(eventData.position);

                int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
                int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);

                InventoryUI inventoryUI = GetComponentInParent<InventoryUI>();

                if (droppedItemUI != null && slot != null && slot.IsEmpty())
                {
                    //slot.PlaceItem(droppedItemUI);
                } else if (inventoryUI != null && inventoryUI.CanPlaceItemAtPosition(droppedItemUI, row, col))
                {
                    inventoryUI.PlaceItem(droppedItemUI, row, col);
                } else
                {
                    Debug.Log("No Space for an Item");
                    ResetItemPosition(droppedItemUI);
                }
            }
        }
    }

    private void ResetItemPosition(ItemUI itemUI)
    {
        RectTransform rectTransform = itemUI.GetComponent<RectTransform>();
        rectTransform.SetParent(itemUI.transform.parent, true);
        Vector2 gridPosition = new Vector2(itemUI.GridPosition.x * InventoryUI.CellSize, -itemUI.GridPosition.y * InventoryUI.CellSize);
        rectTransform.anchoredPosition = gridPosition;
    }
}
