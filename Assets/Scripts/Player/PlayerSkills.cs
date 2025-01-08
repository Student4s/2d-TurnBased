
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Entity entity;//player
    [SerializeField] private Entity target;
    [SerializeField] private MoveOnTilemap playerMove;

    private void Update()// autoattack by click on enemy
    {
        //Click on enemy and move to it
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                target = hit.collider.GetComponent<Entity>();
                Transform enemyTransform = hit.collider.transform;

                playerMove.GoToEnemy(enemyTransform);
                if (playerMove.path.Count<=1)
                {
                    UseSkill("attack");
                }
                else
                {
                    playerMove.GoToEnemy(enemyTransform);
                }
                
            }
        }
    }
    public void UseSkill(string skillName)
    {
        switch (skillName)
        {
            case "attack":
                Attack();
                break;
            case "Charge":
                Charge();
                break;
            default:
                Debug.Log("Unknown skill");
                break;
        }
    }
    // SKILL ZONE
    public void Attack()
    {
        target.GetDamage(entity.damage);
        entity.ChangeTurn();
    }
    public void Charge()
    {
        Debug.Log("Charge");
        entity.ChangeTurn();
    }

}
