using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkillList : MonoBehaviour
{
    [SerializeField] private Entity entity2;
    public void Attack(Entity entity)
    {
        entity.GetDamage(entity.damage);
        entity2.ChangeTurn();
    }
}
