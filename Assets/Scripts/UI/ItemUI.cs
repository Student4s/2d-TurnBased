using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour
{
    public Item ItemData { get; private set; } // Logical item data
    public RectTransform Rect { get; private set; } // UI RectTransform
    public Vector2Int GridPosition { get; set; } // Position in the grid
    public ItemDragHandler DragHandler {get; set; }

    private Image itemImage;

    public void Initialize(Item itemData, Sprite icon)
    {
        ItemData = itemData;

        if (Rect == null)
        {
            Rect = gameObject.AddComponent<RectTransform>();
        }

        
        if (itemImage == null)
        {
            itemImage = gameObject.AddComponent<Image>();
        }

        itemImage.sprite = icon;
        Rect.sizeDelta = new Vector2(itemData.cellSize.x * InventoryUI.CellSize, itemData.cellSize.y * InventoryUI.CellSize);
        Rect.anchorMin = new Vector2(0, 1); // Top-left anchor
        Rect.anchorMax = new Vector2(0, 1); // Top-left anchor
        Rect.pivot = new Vector2(0, 1); // Top-left pivot

        if (DragHandler == null){
        DragHandler = gameObject.AddComponent<ItemDragHandler>();
        }

    }

    public void SetPosition(Vector2Int gridPosition, float cellSize)
    {
        GridPosition = gridPosition;

        float xPos = gridPosition.x * cellSize;
        float yPos = -gridPosition.y * cellSize; // Negative due to inverted Y-axis in RectTransform

        Rect.anchoredPosition = new Vector2(xPos, yPos);
    }

    public void SetParent(Transform parent)
    {
        Rect.SetParent(parent, false);
    }
}
