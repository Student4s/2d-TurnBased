using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image image;

    public string skillName = "Aboba";
    public int skillCurrentCooldown = 0;
    public int skillMaxCooldown = 0;
    public float energyCost;

    public Entity player;
    public PlayerSkills skills;
   
    public void Activate()
    {
        if(player.energy>=energyCost && skillCurrentCooldown<=0)
        {
            player.energy -= energyCost;
            skills.UseSkill(skillName);
            skillCurrentCooldown = skillMaxCooldown;
        }
    }

}
