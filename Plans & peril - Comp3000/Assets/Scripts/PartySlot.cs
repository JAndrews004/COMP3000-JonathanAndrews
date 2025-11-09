using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public PartyMember CurrentPartyMember;
    public TextMeshPro NameText;
    public GameObject TargetHighlight;
    public Slider HPBar;
    public Slider ShieldBar;
    private bool HasAssignedSprite = false;
    public GameObject CharacterArrow;
    public GameObject TargetArrow;
    private void Start()
    {
        NameText = GetComponentInChildren<TextMeshPro>();
        if (TargetHighlight != null)
        {
            TargetHighlight.SetActive(false);
        }
        
    }
    private void Update()
    {
        if (CurrentPartyMember != null)
        {
            NameText.text = CurrentPartyMember.baseStats.characterName;
            HPBar.maxValue = CurrentPartyMember.CurrentMaxHealth;
            HPBar.value = CurrentPartyMember.CurrentHealth;
            ShieldBar.maxValue = CurrentPartyMember.CurrentMaxHealth;
            ShieldBar.value = CurrentPartyMember.shieldValue;
            if (!HasAssignedSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentPartyMember.baseStats.characterSprite;
                HasAssignedSprite = true;
            }
        }
    }

    public void TurnCharacterArrowOn()
    {
        CharacterArrow.SetActive(true);
    }
    public void TurnCharacterArrowOff()
    {
        CharacterArrow.SetActive(false);
    }
    public void TurnTargetArrowOn()
    {
        TargetArrow.SetActive(true);
    }
    public void TurnTargetArrowOff()
    {
        TargetArrow.SetActive(false);
    }

}
