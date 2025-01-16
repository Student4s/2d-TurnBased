using UnityEngine;
using UnityEngine.UI;

public class ControllerUI : MonoBehaviour
{
    private GameObject canvasObject; // Shared canvas for all UI elements
    private InventoryUI inventoryUI;

    void Awake()
    {
        // Ensure the canvas is initialized when the game starts
        CreateCanvas();
    }

    void Update()
    {
        // Toggle inventory UI with the "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }
    }

    void ToggleInventoryUI()
    {
        if (inventoryUI == null)
        {
            CreateInventoryUI();
        }
        else
        {
            if (inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.Hide();
            }
            else
            {
                inventoryUI.Show();
            }
        }
    }


    private void CreateCanvas()
    {
        if (canvasObject == null)
        {
            canvasObject = new GameObject("CanvasUI");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();
        }
    }

    // Method to create the InventoryUI
    private void CreateInventoryUI()
    {

        // Create the Inventory UI object
        GameObject inventoryObject = new GameObject("InventoryUI");
        inventoryObject.transform.SetParent(canvasObject.transform, false);

        RectTransform rectTransform = inventoryObject.AddComponent<RectTransform>();
        inventoryUI = inventoryObject.AddComponent<InventoryUI>();
        

        // Initialize the InventoryUI on the right side
        inventoryUI.InitializeUI(
            rectTransform,
            new Vector2(1, 0), // Anchor Min (right side)
            new Vector2(1, 1), // Anchor Max (right side)
            new Vector2(1, 0.5f), // Pivot (center-right)
            new Vector2(300, 0) // SizeDelta (300px width, full height)
        );
    }
}
