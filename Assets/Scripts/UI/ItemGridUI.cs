// ItemGridUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ItemGridUI : MonoBehaviour
{
    public bool[,] OccupiedCells { get; private set; }
    public RectTransform GridPanel { get; private set; }
    public float CellSize { get; private set; }

    private int gridRows;
    private int gridColumns;

    public void Initialize(int rows, int columns, float cellSize, RectTransform parentPanel)
    {
        gridRows = rows;
        gridColumns = columns;
        OccupiedCells = new bool[rows, columns];
        CellSize = cellSize;
        //CreateGrid(parentPanel);
        GridPanel = parentPanel;
        CreateGridCells();
    }

    private void CreateGrid(RectTransform parentPanel)
    {
        // Create a child panel to hold the grid
        GameObject gridPanelObject = new GameObject("GridPanel", typeof(RectTransform), typeof(Image));
        GridPanel = gridPanelObject.GetComponent<RectTransform>();
        gridPanelObject.transform.SetParent(parentPanel, false);
        

        // Configure the grid panel to occupy the lower half of the parent panel
        GridPanel.anchorMin = new Vector2(0, 0);
        GridPanel.anchorMax = new Vector2(1, 0.5f);
        GridPanel.sizeDelta = Vector2.zero;
        GridPanel.pivot = new Vector2(0, 1);

        Image panelImage = GridPanel.GetComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.5f);

        // CalculateCellSize(parentPanel);
        CreateGridCells();
    }


    private void CreateGridCells()
    {
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                GameObject cell = new GameObject($"Cell_{row}_{col}", typeof(RectTransform), typeof(Image), typeof(Outline));
                RectTransform cellRect = cell.GetComponent<RectTransform>();
                cell.transform.SetParent(GridPanel, false);

                cellRect.sizeDelta = new Vector2(CellSize, CellSize);
                cellRect.anchorMin = new Vector2(0, 1);
                cellRect.anchorMax = new Vector2(0, 1);
                cellRect.pivot = new Vector2(0, 1);

                float yPos = -(CellSize * row);
                float xPos = CellSize * col;
                cellRect.anchoredPosition = new Vector2(xPos, yPos);

                Image cellImage = cell.GetComponent<Image>();
                cellImage.color = new Color(0.8f, 0.8f, 0.8f, 1);

                Outline outline = cell.GetComponent<Outline>();
                outline.effectColor = new Color(0, 0, 0, 1);
                outline.effectDistance = new Vector2(2, 2);
            }
        }
    }

    public bool CanPlaceItemAtPosition(ItemUI itemUI, int startRow, int startCol)
    {
        for (int r = 0; r < itemUI.ItemData.cellSize.y; r++)
        {
            for (int c = 0; c < itemUI.ItemData.cellSize.x; c++)
            {
                int targetRow = startRow + r;
                int targetCol = startCol + c;

                if (targetRow >= gridRows || targetCol >= gridColumns || OccupiedCells[targetRow, targetCol])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlaceItem(ItemUI itemUI, int startRow, int startCol)
    {
        itemUI.SetParent(GridPanel);
        itemUI.SetPosition(new Vector2Int(startCol, startRow), CellSize);

        for (int r = 0; r < itemUI.ItemData.cellSize.y; r++)
        {
            for (int c = 0; c < itemUI.ItemData.cellSize.x; c++)
            {
                OccupiedCells[startRow + r, startCol + c] = true;
            }
        }

        Debug.Log($"Item '{itemUI.ItemData.itemName}' placed at grid position ({startRow}, {startCol}).");
    }

    public void ClearCells(ItemUI itemUI, int startRow, int startCol)
    {
        for (int r = 0; r < itemUI.ItemData.cellSize.y; r++)
        {
            for (int c = 0; c < itemUI.ItemData.cellSize.x; c++)
            {
                int targetRow = startRow + r;
                int targetCol = startCol + c;

                if (targetRow < gridRows || targetCol < gridColumns)
                {
                    OccupiedCells[targetRow, targetCol] = false;
                }
            }
        }
    }
}
