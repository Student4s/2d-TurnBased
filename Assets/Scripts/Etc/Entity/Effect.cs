using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public string effectType;// for onehitEffects.
    public int turnCounts;
    public string effectName;
    public int effectPower;// stacks of effect

    public HpSystem target;

    public void InitiateEffect(string names, int turns, int power, HpSystem target1)
    {
        effectName = names;
        turnCounts = turns;
        effectPower = power;
        target = target1;
    }

    public void EffectUsing()
    {
        switch(effectName)
        {
            case "bleeding":
                Bleeding();
                break;
            default:
                Debug.Log("Unknown effect");
                break;
        }
            
    }

    public void EffectEnd()
    {
        switch (effectName)
        {
            default:
                Debug.Log("Lol, do nothing");
                break;
        }
    }
    /// EFFECTS ZONE
    void Bleeding()
    {
        target.GetDamage(effectPower, "bleeding");
        turnCounts -= 1;
    }

}
