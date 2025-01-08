using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    public Entity entity;
    [SerializeField] private ArmorSystem armor;
    public List<Effect> effects;

    private void Start()
    {
        entity = gameObject.GetComponent<Entity>();
        armor = gameObject.GetComponent<ArmorSystem>();

        //Effect a = new Effect();
        //a.InitiateEffect("bleeding", 3, 4, this);
        //effects.Add(a);
    }
    public void GetDamage(float damage, string damageType)
    {
        entity.hp -= damage*(10-armor.armor.armor)/10;
        Debug.Log(damageType);
        entity.hpBar.gameObject.SetActive(true);
        entity.hpBar.fillAmount = entity.hp / entity.maxHP; 
        if (entity.hp<=0)
        {
            Die();
        }
    }

    public void CheckEffects()
    {
        for(int i=0; i<effects.Count; i++)
        {
            effects[i].EffectUsing();
            if (effects[i].turnCounts<=0)
            {
                effects[i].EffectEnd();
                effects.Remove(effects[i]);
            }
        }
    }

    void Die()
    {
        Debug.Log("Die");
    }
}
