using System.Data;
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


    private Vector2 originalPosition;
    private Transform originalParent;

    private GameObject shadowObject;
    private RectTransform shadowRect;
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
        if (itemUI == null || inventoryUI == null) return;

        SaveOriginalState();

        SlotUI originalSlot = GetComponentInParent<SlotUI>();
        // originalSlot?.RemoveItem();

        if (originalSlot != null)
        {
            originalSlot.RemoveItem();
        }
        else
        {
            inventoryUI.itemGridUI.ClearCells(itemUI, GetGridRow(), GetGridCol());
        }

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
    private void CreateShadow()
    {
        shadowObject = new GameObject("Shadow");
        shadowRect = shadowObject.AddComponent<RectTransform>();
        shadowObject.AddComponent<CanvasRenderer>();

        shadowObject.transform.SetParent(canvas.transform, false);
        shadowRect.sizeDelta = new Vector2(itemUI.ItemData.cellSize.x * InventoryUI.CellSize, itemUI.ItemData.cellSize.y * InventoryUI.CellSize);
        shadowRect.pivot = rectTransform.pivot;
        shadowRect.anchorMin = rectTransform.anchorMin;
        shadowRect.anchorMax = rectTransform.anchorMax;

        Image shadowImage = shadowObject.AddComponent<Image>();

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
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            SlotOnDropHandler slotHandler = result.gameObject.GetComponent<SlotOnDropHandler>();
            if (slotHandler != null)
            {
                RectTransform slotRect = result.gameObject.GetComponent<RectTransform>();
                shadowRect.sizeDelta = slotRect.sizeDelta;
                shadowObject.transform.SetParent(slotRect, false);
                shadowObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                shadowObject.transform.SetParent(inventoryUI.transform, true);
                rectTransform.SetAsLastSibling();
                shadowObject.SetActive(true);


                return;


            }

            ItemGridUI itemGridUI = result.gameObject.GetComponent<ItemGridUI>();
            if (itemGridUI != null)
            {

                RectTransform gridRect = result.gameObject.GetComponent<RectTransform>();
                shadowObject.transform.SetParent(gridRect, false);

                shadowRect.sizeDelta = new Vector2(itemUI.ItemData.cellSize.x * InventoryUI.CellSize, itemUI.ItemData.cellSize.y * InventoryUI.CellSize);


                Vector2 localPosition = result.gameObject.transform.InverseTransformPoint(eventData.position);
                int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
                int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);


                //shadowRect.position = new Vector3(row * InventoryUI.CellSize, col * InventoryUI.CellSize, 0);
                shadowRect.anchoredPosition = new Vector2(col * InventoryUI.CellSize, -row * InventoryUI.CellSize);
                Debug.Log (shadowRect.position.x + " " + shadowRect.position.x + " " + InventoryUI.CellSize);

                // shadowObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // shadowObject.transform.SetParent(inventoryUI.transform, true);
                // rectTransform.SetAsLastSibling();
                shadowObject.SetActive(true);
                return;
            }
        }

        shadowObject.SetActive(false);
    }

    private bool TryPlaceItem(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            SlotUI targetSlot = result.gameObject.GetComponent<SlotUI>();
            if (targetSlot != null && targetSlot.IsEmpty())
            {
                targetSlot.PlaceItem(itemUI);
                return true;
            }

            ItemGridUI targetGrid = result.gameObject.GetComponent<ItemGridUI>();

            if (targetGrid != null)
            {
                Vector2 localPosition = result.gameObject.transform.InverseTransformPoint(eventData.position);
                int row = Mathf.FloorToInt(-localPosition.y / InventoryUI.CellSize);
                int col = Mathf.FloorToInt(localPosition.x / InventoryUI.CellSize);

                if (inventoryUI.itemGridUI.CanPlaceItemAtPosition(itemUI, row, col))
                {
                    inventoryUI.itemGridUI.PlaceItem(itemUI, row, col);
                    return true;
                }
            }
        }

        return false;
    }

    private int GetGridRow()
    {
        return Mathf.FloorToInt(-originalPosition.y / InventoryUI.CellSize);
    }

    private int GetGridCol()
    {
        return Mathf.FloorToInt(originalPosition.x / InventoryUI.CellSize);
    }
}
