using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomSkillBar : MonoBehaviour
{
    public List<SkillSlot> skills;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //key1
        {
            Debug.Log("aboba");
            if (skills[0].skillName !=null)
            {
                skills[0].Activate();
            }
        }
    }

    public void SkillCooldown()
    {
        for (int i =0; i<skills.Count;i++)
        {
            skills[i].skillCurrentCooldown -= 1;
        }
    }
}
