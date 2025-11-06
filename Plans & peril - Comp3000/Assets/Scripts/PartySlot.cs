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
    private bool HasAssignedSprite = false;

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
            if (!HasAssignedSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentPartyMember.baseStats.characterSprite;
                HasAssignedSprite = true;
            }
        }
    }
}
