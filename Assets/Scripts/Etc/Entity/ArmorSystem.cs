using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSystem : MonoBehaviour
{
    public List<Slot> armors;
    public List<Armor> armorsInGame;// crutch but idk how to make straight
    public Armor armor;// summary armor. using in damage resist

    private void Start()
    {
        UpdateArmor();
    }
    private void OnEnable()
    {
        ItemDragHandler.RecalculateArmor += UpdateArmor;
        
    }

    private void OnDisable()
    {
        ItemDragHandler.RecalculateArmor -= UpdateArmor;
    }
    public void UpdateArmor()
    {
        armor.ZeroArmor();
        for (int i = 0; i < armors.Count; i++)
        {
            if(armors[i].currentItem!=null)
            {
                armor.AddArmor(armorsInGame[armors[i].currentItem.id]);//change second ID on armor;
            }
            
        }
    }


}
