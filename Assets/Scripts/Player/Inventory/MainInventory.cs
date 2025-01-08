using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    [SerializeField] private MoveOnTilemap player;
    public List<Slot> slots;
    [SerializeField] private GameObject inventoryCanvas;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inventoryCanvas.SetActive(true);
            player.ChangeCanMove(false);
        }
    }
}
