using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private InventoryUI inventoryUI;
    private ItemUI itemUI;

    private Vector2 originalPosition;
    private Transform originalParent;
    
    private GameObject shadowObject;
    private RectTransform shadowRect;

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
        if (itemUI == null || inventoryUI == null) return;

        SaveOriginalState();
        RemoveItemFromSlot();
        
        rectTransform.SetParent(inventoryUI.transform, true);
        canvasGroup.blocksRaycasts = false;
        
        CreateShadow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        UpdateShadowPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        DestroyShadow();
        
        if (!TryPlaceItem(eventData))
        {
            ResetToOriginalState();
        }
    }

    private void SaveOriginalState()
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
    }

    private void ResetToOriginalState()
    {
        rectTransform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
    }

    private void RemoveItemFromSlot()
    {
        SlotUI originalSlot = GetComponentInParent<SlotUI>();
        if (originalSlot != null)
        {
            originalSlot.RemoveItem();
        }
        else
        {
            inventoryUI.itemGridUI.ClearCells(itemUI, GetGridRow(), GetGridCol());
        }
    }

    private void CreateShadow()
    {
        shadowObject = new GameObject("Shadow", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        shadowRect = shadowObject.GetComponent<RectTransform>();
        
        shadowObject.transform.SetParent(canvas.transform, false);
        shadowRect.sizeDelta = new Vector2(itemUI.ItemData.cellSize.x * InventoryUI.CellSize, itemUI.ItemData.cellSize.y * InventoryUI.CellSize);
        shadowRect.pivot = rectTransform.pivot;
        shadowRect.anchorMin = rectTransform.anchorMin;
        shadowRect.anchorMax = rectTransform.anchorMax;

        Image shadowImage = shadowObject.GetComponent<Image>();
        shadowImage.color = new Color(0, 0, 0, 0.2f);
        shadowImage.raycastTarget = false;
        shadowObject.SetActive(false);
    }

    private void DestroyShadow()
    {
        if (shadowObject != null)
        {
            Destroy(shadowObject);
        }
    }

    private void UpdateShadowPosition(PointerEventData eventData)
    {
        shadowObject.SetActive(false);
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (TryUpdateShadowForSlot(result) || TryUpdateShadowForGrid(result, eventData))
            {
                return;
            }
        }
    }

    private bool TryUpdateShadowForSlot(RaycastResult result)
    {
        SlotOnDropHandler slotHandler = result.gameObject.GetComponent<SlotOnDropHandler>();
        if (slotHandler == null) return false;

        RectTransform slotRect = result.gameObject.GetComponent<RectTransform>();
        shadowRect.sizeDelta = slotRect.sizeDelta;
        shadowObject.transform.SetParent(slotRect, false);
        shadowRect.anchoredPosition = Vector2.zero;
        shadowObject.transform.SetParent(inventoryUI.transform, true);
        rectTransform.SetAsLastSibling();
        shadowObject.SetActive(true);

        return true;
    }

    private bool TryUpdateShadowForGrid(RaycastResult result, PointerEventData eventData)
    {
        ItemGridUI itemGridUI = result.gameObject.GetComponent<ItemGridUI>();
        if (itemGridUI == null) return false;

        shadowObject.transform.SetParent(result.gameObject.transform, false);
        shadowRect.sizeDelta = new Vector2(itemUI.ItemData.cellSize.x * InventoryUI.CellSize, itemUI.ItemData.cellSize.y * InventoryUI.CellSize);
        
        Vector2 localPosition = result.gameObject.transform.InverseTransformPoint(eventData.position);
        int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
        int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);
        shadowRect.anchoredPosition = new Vector2(col * InventoryUI.CellSize, -row * InventoryUI.CellSize);

        shadowObject.SetActive(true);
        return true;
    }

    private bool TryPlaceItem(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (TryPlaceInSlot(result) || TryPlaceInGrid(result, eventData))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryPlaceInSlot(RaycastResult result)
    {
        SlotUI targetSlot = result.gameObject.GetComponent<SlotUI>();
        if (targetSlot == null || !targetSlot.IsEmpty()) return false;

        targetSlot.PlaceItem(itemUI);
        return true;
    }

    private bool TryPlaceInGrid(RaycastResult result, PointerEventData eventData)
    {
        ItemGridUI targetGrid = result.gameObject.GetComponent<ItemGridUI>();
        if (targetGrid == null) return false;

        Vector2 localPosition = result.gameObject.transform.InverseTransformPoint(eventData.position);
        int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
        int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);

        if (!inventoryUI.itemGridUI.CanPlaceItemAtPosition(itemUI, row, col)) return false;

        inventoryUI.itemGridUI.PlaceItem(itemUI, row, col);
        return true;
    }

    private int GetGridRow() => Mathf.FloorToInt(-originalPosition.y / InventoryUI.CellSize);
    private int GetGridCol() => Mathf.FloorToInt(originalPosition.x / InventoryUI.CellSize);
}
