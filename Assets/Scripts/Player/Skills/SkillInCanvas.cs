using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillInCanvas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string skillName;
    public int skillMaxCooldown = 0;
    public float energyCost;

    public bool isLearned;
    public int price;
    public Sprite image;
    public List<SkillInCanvas> requiredSkills;

    public CrutchForInventorySprite dragSprite2;

    void Start()
    {
        dragSprite2.spriterender.enabled = false;
    }

    public bool CanLearnSkill()
    {
        if(!isLearned)
        {
            bool a = true;
            for (int i = 0; i < requiredSkills.Count; i++)
            {
                if (!requiredSkills[i].isLearned)
                {
                    a = false;
                }
            }
            return a;
        }
        else
        {
            return false;
        }
    }
    // DRAG & DROP ZONE

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isLearned)
        {
            dragSprite2.spriterender.enabled = true;
            dragSprite2.sprite = image;
            dragSprite2.spriterender.sprite = dragSprite2.sprite;
            //dragSprite2.transform.parent = transform.root;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;
        dragSprite2.transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragSprite2.spriterender.enabled = false;
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<SkillSlot>() != null)
        {
            SkillSlot newSlot = eventData.pointerEnter.GetComponent<SkillSlot>();
            newSlot.skillName = skillName;
            newSlot.skillMaxCooldown = skillMaxCooldown;
            newSlot.skillCurrentCooldown = skillMaxCooldown;
            newSlot.energyCost = energyCost;
            newSlot.image.sprite = image;
        }
    }
}
