using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string itemType;
    public int id;

    public bool isUsable;
    public int countOfUse;
    public int idForUse;
}
