using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementSlot : MonoBehaviour, IDropHandler
{
    
    public Image Icon;
    public AbilityData equipedAbility;
    public CharacterSkillTree characterSkillTree;

    public GameObject floatingIconPrefab;
    public GameObject floatingIcon;
    public GameObject canvas;
    public SkillButton skillButtonOnSlot;


    public void OnDrop(PointerEventData eventData)
    {
        
        if (eventData.pointerDrag.GetComponent<SkillButton>())
        {
            SkillButton skillButton = eventData.pointerDrag.GetComponent<SkillButton>();
            PartyMember member = characterSkillTree.pm;
            if (skillButton.ability != null && skillButton.ability.isElement)
            {
                
                if (skillButton.ability.unlocked)
                {
                    Debug.Log("Button dropped");
                    equipedAbility = skillButton.ability;
                    member.EquipElement(skillButton.ability);
                    
                }
            }


        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        characterSkillTree = GetComponentInParent<SkillsMenu>().chosenCharacter.characterSkillTree;
        if (equipedAbility != null)
        {
            Icon.sprite = equipedAbility.icon;
            
        }
        else
        {
            equipedAbility = characterSkillTree.pm.baseStats.EquippedElement;
            Icon.sprite = null;
        }
        if (characterSkillTree.pm != null)
        {
            
            skillButtonOnSlot.ability = equipedAbility;
            

        }

    }
    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipedAbility != null)
        {
            if (equipedAbility.unlocked)
            {
                //create floating icon (prefab with alpha lower) and set floating icons image to ability.icon
                floatingIcon = Instantiate(floatingIconPrefab, canvas.transform);
                floatingIcon.GetComponent<Image>().sprite = equipedAbility.icon;
                floatingIcon.GetComponent<Image>().raycastTarget = false;

            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        //move floating icon with the mouse
        if (equipedAbility != null && floatingIcon!=null)
        {
            floatingIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(floatingIcon);
        floatingIcon = null;
    }
    */
}


