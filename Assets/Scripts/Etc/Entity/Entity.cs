using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public float maxHP;
    public float hp;
    public Image hpBar;
    public HpSystem hpSystem;

    public float maxEnergy;
    public float energy;
    public Image energyBar;

    public float damage;
    public bool turn;

    public delegate void EnityTurnChange();
    public static event EnityTurnChange End;

    private void OnEnable()
    {
        hpSystem = gameObject.GetComponent<HpSystem>();
    }
    public void ChangeTurn()
    {

        turn = false;
        if(!turn)
        {
            End();
        }
    }

    public void GetDamage(float damage)
    {
        hpSystem.GetDamage(damage, "aboba");
    }

    public void Die()
    {
        Debug.Log("Die");
    }
}

