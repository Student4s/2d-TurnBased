using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; } // Position in the grid
    public string SlotType { get; private set; } // For equip panel slots (e.g., Head, Armor)
    public ItemUI ContainedItem { get; private set; } // Reference to the item in this slot
   public SlotOnDropHandler DropHandler {get; private set;}

    private Image slotImage;

    public void Initialize(Vector2Int gridPosition, string slotType = null)
    {
        GridPosition = gridPosition;
        SlotType = slotType;

        if (slotImage == null)
        {
            slotImage = gameObject.AddComponent<Image>();
        }

        // Set default visual properties for the slot
        slotImage.color = new Color(0.8f, 0.8f, 0.8f, 1); // Light gray

        if(DropHandler == null){
            DropHandler = gameObject.AddComponent<SlotOnDropHandler>();
        }
    }

    public bool IsEmpty()
    {
        return ContainedItem == null;
    }

    public void PlaceItem(ItemUI item)
    {
        if (!IsEmpty())
        {
            Debug.LogError("Slot is already occupied.");
            return;
        }

        ContainedItem = item;
        item.SetParent(transform);
        item.SetPosition(GridPosition, InventoryUI.CellSize);
    }

    public void RemoveItem()
    {
        ContainedItem = null;
    }

    public void GetItem(){
        Debug.Log("Current item: " + ContainedItem.name);
    }
}
