using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnChanger : MonoBehaviour
{
    [SerializeField] private List<Entity> enemies;
    [SerializeField] private Entity player;
    [SerializeField] private BottomSkillBar playerSkills;
    [SerializeField] private bool turn=true;// true - allies, false - enemies

    private void OnEnable()
    {
        Entity.End += ChangeTurn;
    }

    private void OnDisable()
    {
        Entity.End -= ChangeTurn;
    }

    void ChangeTurn()
    {

        if(turn && !player.turn && enemies.Count > 0)// refresh all enemmies turns after player move
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].turn = true;
                enemies[i].hpSystem.CheckEffects();
                enemies[i].GetComponent<EnemyTest1>().currentTimeBetweenSteps=0.2f;//delay before move
            }
            turn = false;
            return;
        }

        if (enemies.Count==0)//fastMove if no enemy in vision;
        {
            player.turn = true;
            player.hpSystem.CheckEffects();
            playerSkills.SkillCooldown();
            turn = true;
            return;
        }

        if(!turn && !player.turn && enemies.Count > 0)// player turn after all enemy go
        {
            bool a = true;
            for(int i = 0; i < enemies.Count; i++)// check all enemy made turn
            {
                if (enemies[i].turn)
                {
                    a = false;
                }
            }
            if(a)
            {
                player.GetComponent<MoveOnTilemap>().path.Clear();
                player.turn = true;
                player.hpSystem.CheckEffects();
                playerSkills.SkillCooldown();
                turn = true;
            }
            return;
        }
    }
}
