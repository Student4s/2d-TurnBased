using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class InventoryUI : BaseUI
{
    public RectTransform inventoryPanel;
    
    public ItemGridUI itemGridUI;
    public Dictionary<string, SlotUI> equipSlots;

    [SerializeField] private const int gridRows = 5;
    [SerializeField] private const int gridColumns = 12;

    static public float CellSize { get; private set; } = -1f;

    void Start()
    {

        if (inventoryPanel == null)
        {
            inventoryPanel = GetComponent<RectTransform>();
        }

        CalculateCellSize();


        equipSlots = new Dictionary<string, SlotUI>();
        CreateEquipPanel();

        CreateGridPanel();
        

        AddDebugItems();
    }


    private void AddDebugItems()
    {
        for (int i = 1; i <= 4; i++)
        {
            Item debugItem = ScriptableObject.CreateInstance<Item>();
            debugItem.name = "Debug Item " + i;

            CreateItemUI(debugItem, new Vector2Int(i, i));
        }
    }

    public void CreateItemUI(Item item, Vector2Int size)
    {
        item.cellSize = size;
        GameObject itemObject = new GameObject(item.itemName);



        // Attach the item object to the grid panel
        itemObject.transform.SetParent(itemGridUI.GridPanel, false);



        // Now add and initialize the ItemUI component
        ItemUI itemUI = itemObject.AddComponent<ItemUI>();
        itemUI.Initialize(item, item.icon);



        AddItemToGrid(itemUI);
    }

    public void AddItemToGrid(ItemUI itemUI)
    {
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                if (itemGridUI.CanPlaceItemAtPosition(itemUI, row, col))
                {
                    itemGridUI.PlaceItem(itemUI, row, col);
                    return;
                }
            }
        }

        Debug.LogWarning("No empty space found for the item.");
    }




    private void CalculateCellSize()
    {
        float panelWidth = inventoryPanel.rect.width;
        float panelHeight = inventoryPanel.rect.height;
        CellSize = Mathf.Min(panelWidth / gridColumns, panelHeight / gridRows);
    }

    // private void CreateInventoryGrid()
    // {

    //     // Create a child panel to hold the grid
    //     GameObject gridPanelObject = new GameObject("GridPanel", typeof(RectTransform), typeof(Image));
    //     gridPanel = gridPanelObject.GetComponent<RectTransform>();
    //     gridPanelObject.transform.SetParent(inventoryPanel, false);

    //     // Configure the grid panel to occupy the lower half of the parent panel
    //     gridPanel.anchorMin = new Vector2(0, 0);
    //     gridPanel.anchorMax = new Vector2(1, 0.5f);
    //     gridPanel.sizeDelta = Vector2.zero; // Reset sizeDelta for proper anchoring
    //     gridPanel.pivot = new Vector2(0, 1);

    //     // Optional: Add a background color or texture to the grid panel
    //     Image panelImage = gridPanelObject.GetComponent<Image>();
    //     panelImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black


    //     // Loop to create the grid
    //     for (int row = 0; row < gridRows; row++)
    //     {
    //         for (int col = 0; col < gridColumns; col++)
    //         {
    //             // Create a new cell GameObject
    //             GameObject cell = new GameObject($"Cell_{row}_{col}", typeof(RectTransform), typeof(Image), typeof(Outline), typeof(ItemDropHandler));
    //             RectTransform cellRect = cell.GetComponent<RectTransform>();
    //             cell.transform.SetParent(gridPanel, false);

    //             // Configure the cell's RectTransform
    //             cellRect.sizeDelta = new Vector2(CellSize, CellSize);
    //             cellRect.anchorMin = new Vector2(0, 1);
    //             cellRect.anchorMax = new Vector2(0, 1);
    //             cellRect.pivot = new Vector2(0, 1);

    //             // Set the position of the cell
    //             float yPos = -(CellSize * row); // Negative because the Y-axis in RectTransform is inverted
    //             float xPos = CellSize * col;   // Standard positioning for X
    //             cellRect.anchoredPosition = new Vector2(xPos, yPos);

    //             // Add an optional background color to the cell
    //             Image cellImage = cell.GetComponent<Image>();
    //             cellImage.color = new Color(0.8f, 0.8f, 0.8f, 1); // Light gray

    //             // Add an outline to the cell
    //             Outline outline = cell.GetComponent<Outline>();
    //             outline.effectColor = new Color(0, 0, 0, 1); // Black outline
    //             outline.effectDistance = new Vector2(2, 2); // Adjust thickness as needed

    //         }
    //     }


    //     Debug.Log("Grid was succesfully created.");
    // }

    public void CreateGridPanel(){

                // Validate the parent panel
        if (inventoryPanel == null)
        {
            Debug.LogError("Parent panel is not assigned.");
            return;
        }

        if (CellSize == -1f)
        {
            Debug.LogWarning("Cell size not set, calculating now.");
            CalculateCellSize();
        }

                // Create a child panel to hold the grid
        GameObject gridPanelObject = new GameObject("GridPanel", typeof(RectTransform), typeof(Image), typeof(ItemGridUI));
        RectTransform gridPanel = gridPanelObject.GetComponent<RectTransform>();
        gridPanelObject.transform.SetParent(inventoryPanel, false);

        // Configure the grid panel to occupy the lower half of the parent panel
        gridPanel.anchorMin = new Vector2(0, 0);
        gridPanel.anchorMax = new Vector2(1, 0.5f);
        gridPanel.sizeDelta = Vector2.zero; // Reset sizeDelta for proper anchoring
        gridPanel.pivot = new Vector2(0, 1);

        // Optional: Add a background color or texture to the grid panel
        Image panelImage = gridPanelObject.GetComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black

        itemGridUI = gridPanelObject.GetComponent<ItemGridUI>();
        itemGridUI.Initialize(gridRows, gridColumns, CellSize, gridPanel);
    }

    public void CreateEquipPanel()
    {
        // Validate the parent panel
        if (inventoryPanel == null)
        {
            Debug.LogError("Parent panel is not assigned.");
            return;
        }

        if (CellSize == -1f)
        {
            Debug.LogWarning("Cell size not set, calculating now.");
            CalculateCellSize();
        }

        // Create a child panel to hold the equipment slots
        GameObject equipPanelObject = new GameObject("EquipPanel", typeof(RectTransform), typeof(Image));
        RectTransform equipPanel = equipPanelObject.GetComponent<RectTransform>();
        equipPanelObject.transform.SetParent(inventoryPanel, false);

        // Configure the equip panel to occupy the upper half of the parent panel
        equipPanel.anchorMin = new Vector2(0, 0.5f);
        equipPanel.anchorMax = new Vector2(1, 1);
        equipPanel.sizeDelta = Vector2.zero; // Reset sizeDelta for proper anchoring
        equipPanel.pivot = new Vector2(0.5f, 0.5f);

        // Optional: Add a background color or texture to the equip panel
        Image panelImage = equipPanelObject.GetComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.5f); // Semi-transparent dark background

        GameObject contentPanel = new GameObject("Content");
        contentPanel.transform.SetParent(equipPanel.transform, false);

        float baseSlotSize = CellSize; // Adjust as needed


        var slotDefinitions = new (string, Vector2, Vector2)[]
           {
        ("Head", 
        new Vector2(0.5f, 0.5f + 0.25f), 
        new Vector2(baseSlotSize*2, baseSlotSize*2)),
        ("Armor", 
        new Vector2(0.5f, 0.5f - 0.05f), 
        new Vector2(baseSlotSize*2, baseSlotSize * 3)),
        ("WeaponLeft", 
        new Vector2(0.5f - 0.3f, 0.5f + 0.12f), 
        new Vector2(baseSlotSize*2, baseSlotSize * 4)),
        ("WeaponRight", 
        new Vector2(0.5f + 0.3f, 0.5f + 0.12f),
         new Vector2(baseSlotSize*2, baseSlotSize * 4)),
        ("Ring1", 
        new Vector2(0.5f - 0.15f, 0.5f - 0.05f), 
        new Vector2(baseSlotSize, baseSlotSize)),
        ("Ring2", 
        new Vector2(0.5f + 0.15f, 0.5f - 0.05f), 
        new Vector2(baseSlotSize, baseSlotSize)),
        ("Neck", 
        new Vector2(0.5f + 0.15f, 0.5f + 0.2f), 
        new Vector2(baseSlotSize, baseSlotSize)),
        ("Gloves", 
        new Vector2(0.5f - 0.2f, 0.5f - 0.25f), 
        new Vector2(baseSlotSize*2, baseSlotSize*2)),
        ("Boots", 
        new Vector2(0.5f + 0.2f, 0.5f - 0.25f), 
        new Vector2(baseSlotSize*2, baseSlotSize*2)),
        ("Belt", 
        new Vector2(0.5f, 0.5f - 0.3f), 
        new Vector2(baseSlotSize*2, baseSlotSize)),
        };

        foreach (var (name, anchorPos, size) in slotDefinitions)
        {
            CreateSlot(equipPanel, anchorPos, name, size);
        }

        Debug.Log("Equipment panel created dynamically.");
    }

    private void CreateSlot(RectTransform parent, Vector2 anchorPosition, string slotType, Vector2 size)
    {
        GameObject slotObject = new GameObject(slotType, typeof(RectTransform), typeof(SlotUI));
        RectTransform slotRect = slotObject.GetComponent<RectTransform>();
        slotObject.transform.SetParent(parent, false);

        slotRect.anchorMin = anchorPosition;
        slotRect.anchorMax = anchorPosition;
        slotRect.sizeDelta = size;
        slotRect.pivot = new Vector2(0.5f, 0.5f);

        SlotUI slotUI = slotObject.GetComponent<SlotUI>();
        slotUI.Initialize(Vector2Int.zero, slotType); // Positioning is only logical in grid-based slots

        equipSlots.Add(slotType, slotUI);
    }




}
