using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private InventoryUI inventoryUI;

    private Vector2 originalPosition;
    private Transform originalParent;

    private GameObject shadowObject;
    private ItemUI itemUI;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (canvas == null)
        {
            Debug.LogError("Canvas not found. Ensure the object is under a Canvas.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemUI = GetComponent<ItemUI>();

        // Save the original position and parent
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;


        

        // Calculate grid position
        int row = Mathf.FloorToInt(-originalPosition.y / InventoryUI.CellSize);
        int col = Mathf.FloorToInt(originalPosition.x / InventoryUI.CellSize);

        inventoryUI.ClearCells(itemUI, row, col);

        SlotUI originalSlot = GetComponentInParent<SlotUI>();
        if (originalSlot != null)
        {
            originalSlot.RemoveItem();
        }

        rectTransform.SetParent(inventoryUI.transform, true);
        canvasGroup.blocksRaycasts = false; // Allow other objects to register the drop

        CreateShadow();
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        // Update shadow position
        UpdateShadowPosition(eventData);


        Vector2 localPosition = originalParent.InverseTransformPoint(eventData.position);

        // Calculate grid position
        int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
        int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);

        Debug.Log("POZ " + row + "NAH " + col + " can place " + inventoryUI.CanPlaceItemAtPosition(itemUI, row, col));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        SlotUI targetSlot = null;

        // Destroy the shadow
        if (shadowObject != null)
        {
            Destroy(shadowObject);
        }

        // Perform a raycast to check for a drop slot
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.GetComponent<ItemDropHandler>())
            {

                targetSlot = result.gameObject.GetComponent<SlotUI>();                // Convert pointer position to local position in grid

                if (targetSlot != null)
                {
                    Debug.Log("Gde" + targetSlot.IsEmpty());

                    if (targetSlot.IsEmpty())
                    {

                        // Place the item directly in the slot and return
                        targetSlot.PlaceItem(GetComponent<ItemUI>());
                        return;
                    }
                }

                Debug.Log("IDI NAHUI EBLAN");

                Vector2 localPosition = originalParent.InverseTransformPoint(eventData.position);

                // Calculate grid position
                int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
                int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);

                Debug.Log("POZ " + localPosition.y + "NAH " + localPosition.x);




                if (inventoryUI.CanPlaceItemAtPosition(itemUI, row, col))
                {
                    // Place the item and exit
                    inventoryUI.PlaceItem(itemUI, row, col);
                    return;
                }
            }
        }

        // If the item cannot be placed, reset position
        rectTransform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
    }

    private void CreateShadow()
    {
        // Create a new shadow GameObject
        shadowObject = new GameObject("Shadow");
        RectTransform shadowRect = shadowObject.AddComponent<RectTransform>();
        shadowObject.AddComponent<CanvasRenderer>();

        // Set shadow's parent to the canvas or a dedicated shadow layer
        shadowObject.transform.SetParent(canvas.transform, false);

        // Copy size from the original object
        shadowRect.sizeDelta = rectTransform.sizeDelta;

        shadowRect.pivot = rectTransform.pivot;
        shadowRect.anchorMin = rectTransform.anchorMin;
        shadowRect.anchorMax = rectTransform.anchorMax;

        // Add a semi-transparent image (shadow effect)
        Image shadowImage = shadowObject.AddComponent<Image>();
        Image originalImage = GetComponent<Image>();
        if (originalImage != null)
        {
            shadowImage.sprite = originalImage.sprite;
            shadowImage.color = new Color(0, 0, 0, 0.2f); // Semi-transparent black
            shadowImage.raycastTarget = false; // Prevent the shadow from blocking raycasts
        }

        shadowObject.SetActive(false); // Hide shadow initially
    }


    private void UpdateShadowPosition(PointerEventData eventData)
    {
        // Perform a raycast to check for a slot under the pointer
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.GetComponent<ItemDropHandler>())
            {
                RectTransform slotRect = result.gameObject.GetComponent<RectTransform>();

                // Position the shadow over the detected slot
                shadowObject.transform.SetParent(slotRect, false);
                shadowObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                shadowObject.transform.SetParent(inventoryUI.transform, true);

                rectTransform.SetAsLastSibling();
                shadowObject.SetActive(true); // Show the shadow
                return;
            }
        }

        // If no valid slot is detected, hide the shadow
        shadowObject.SetActive(false);
    }

}
