using UnityEngine.EventSystems;
using UnityEngine;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Slot assignedSlot;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CrutchForInventorySprite dragSprite2;


    public delegate void ForArmor();// Recalculate armor after every item change;
    public static event ForArmor RecalculateArmor;

    private void Start()
    {
        canvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        dragSprite2.spriterender.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedSlot.currentItem != null)
        {
            dragSprite2.spriterender.enabled = true;
            dragSprite2.sprite = assignedSlot.currentItem.icon;
            dragSprite2.spriterender.sprite = dragSprite2.sprite;
            dragSprite2.transform.parent = transform.root;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (assignedSlot.currentItem != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0;
            dragSprite2.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragSprite2.spriterender.enabled = false;
        if (assignedSlot.currentItem != null)
        {
            if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Slot>() != null)
            {
                Slot newSlot = eventData.pointerEnter.GetComponent<Slot>();
                if (newSlot.CanAcceptItem(assignedSlot.currentItem)) // is slot fit this item
                {
                    newSlot.AddItem(assignedSlot.currentItem);
                    assignedSlot.ClearSlot();
                    dragSprite2.transform.parent = transform;
                    dragSprite2.transform.localPosition = Vector3.zero;
                    RecalculateArmor();
                }
            }
            else
            {
                dragSprite2.transform.parent = transform;
                dragSprite2.transform.localPosition = Vector3.zero;
            }
        }
    }

}
