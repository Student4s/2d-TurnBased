using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAndStats : MonoBehaviour
{
    public MoveOnTilemap player;
    public GameObject skillCanvas;

    public int skillPoints;
    public SkillInCanvas[] skillNumber;// 
    public Text skillpointText;
    public int statsPoints;
    public int[] statsNumber;// 0 - strength, 1 - vitality
    public Text statspointText;
    //public List<SkillInCanvas> skillList;

    

    public Text strengthpointText;
    public Text vitalitypointText;

    private void Start()
    {
        UpdateAllStats();
    }

    public void UpdateAllStats()
    {
        skillpointText.text = "skill point: " + skillPoints.ToString();
        statspointText.text = "stats point: "+statsPoints.ToString();

        strengthpointText.text = statsNumber[0].ToString();
        vitalitypointText.text = statsNumber[1].ToString();

        player.maxHP = statsNumber[1];
        player.maxEnergy = statsNumber[1] * 2;
        player.damage = statsNumber[0];
    }
    public void AddStats(int statNumber)
    {
        if(statsPoints>=1)
        {
            statsNumber[statNumber] += 1;
            statsPoints -= 1;
            UpdateAllStats();
        }
    }

    public void TryLearnSkill(int skillNumb)
    {
        if (skillNumber[skillNumb].CanLearnSkill() && skillNumber[skillNumb].price<= skillPoints)
        {
            skillNumber[skillNumb].isLearned = true;
            skillPoints -= skillNumber[skillNumb].price;
            UpdateAllStats();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            if(!skillCanvas.activeSelf)
            {
                skillCanvas.SetActive(true);
                player.ChangeCanMove(false);
            }
            else
            {
                skillCanvas.SetActive(false);
                player.ChangeCanMove(true);
            }
        }
    }
}
