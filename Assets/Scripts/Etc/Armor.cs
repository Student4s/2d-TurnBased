using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public int armor;

    public void AddArmor(Armor arm)
    {
        armor += arm.armor;
    }

    public void ZeroArmor()
    {
        armor = 0;
    }
}
