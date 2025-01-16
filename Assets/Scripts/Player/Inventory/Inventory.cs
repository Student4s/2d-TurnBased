using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    [SerializeField] private MoveOnTilemap player;
    public List<ItemSlot> slots;
   
    public int[] currentAmmo;// 0 - pistol
    public int[] maxAmmo;

    public bool FindItemInInventory(string itemName)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].currentItem.name == itemName)
            {
                return true;
            }
        }
        return false;
    }
    public int GetAmmo(int count, int ammoType)
    {
        if (currentAmmo[ammoType] >= count)
        {
            currentAmmo[ammoType] -= count;
            return count;
        }
        else
        {
            int a = currentAmmo[ammoType];
            currentAmmo[ammoType] = 0;
            return (a);
        }
    }

}
