using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item currentItem;
    public Image icon;
    public string slotType;
    public Sprite defaultSprite;

    private void Start()
    {
        if (currentItem != null)
        {
            icon.sprite = currentItem.icon;
        }
        else
        {
            icon.sprite = defaultSprite;
        }
    }
    public virtual void AddItem(Item newItem)
    {

        Item newItem1 = Instantiate(newItem);
        if (newItem1 != null)
        {
            currentItem = newItem1;
            icon.sprite = newItem1.icon;
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = defaultSprite;
    }

    public bool CanAcceptItem(Item item)//Can put item in slot or not
    {
        if (slotType == "Any" || slotType == item.itemType)
        {
            if (currentItem == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            if (currentItem.isUsable)
            {
                currentItem.countOfUse -= 1;
                Debug.Log("Aboba");
                if (currentItem.countOfUse <= 0)
                {
                    ClearSlot();
                }
            }
        }
    }
}
