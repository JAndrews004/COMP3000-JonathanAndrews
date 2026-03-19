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
    public GameObject CharacterArrow;
    public GameObject TargetArrow;

    public Transform StatusEffectContainer;
    public GameObject StatusIconPrefab;     
    private Dictionary<Effect, GameObject> ActiveIcons = new Dictionary<Effect, GameObject>();
    [HideInInspector] public Material mat;

    public GameObject CritText;
    public GameObject DodgeText;
    private void Start()
    { 
        //NameText = GetComponentInChildren<TextMeshPro>();
        if (TargetHighlight != null)
        {
            TargetHighlight.SetActive(false);
        }
        if (GetComponent<SpriteRenderer>())
        {
            mat = GetComponent<SpriteRenderer>().material;
        }
        
    }
    private void Update()
    {
        if (CurrentPartyMember == null ||
        CurrentPartyMember.baseStats == null ||
        NameText == null || HPBar == null || ShieldBar == null)
            return;
        if (CurrentPartyMember != null)
        {
            NameText.text = CurrentPartyMember.baseStats.characterName;
            HPBar.maxValue = CurrentPartyMember.CurrentMaxHealth;
            HPBar.value = CurrentPartyMember.CurrentHealth;
            ShieldBar.maxValue = CurrentPartyMember.CurrentMaxHealth;
            ShieldBar.value = CurrentPartyMember.shieldValue;
            if (!gameObject.GetComponent<Animator>()|| CurrentPartyMember.baseStats.controller==null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentPartyMember.baseStats.characterSprite;
                
            }
            if (!gameObject.GetComponent<Animator>().runtimeAnimatorController)
            {
                gameObject.GetComponent<Animator>().runtimeAnimatorController = CurrentPartyMember.baseStats.controller;
                gameObject.GetComponent<Animator>().speed = Random.RandomRange(0.25f, 0.4f);
            }
        }
    }

    public void TurnCharacterArrowOn()
    {
        if(CharacterArrow!=null)
        CharacterArrow.SetActive(true);
    }
    public void TurnCharacterArrowOff()
    {
        if (CharacterArrow != null) 
        CharacterArrow.SetActive(false);
    }
    public void TurnTargetArrowOn()
    {
        if (TargetArrow != null)
        TargetArrow.SetActive(true);
    }
    public void TurnTargetArrowOff()
    {
        if(TargetArrow != null)
        TargetArrow.SetActive(false);
    }

    public void RefreshStatusEffects()
    {
        if (CurrentPartyMember == null) return;

        if (StatusEffectContainer != null)
        {
            // Clear existing icons
            foreach (Transform child in StatusEffectContainer)
                Destroy(child.gameObject);
            ActiveIcons.Clear();
        }
        

        // Rebuild list
        foreach (Effect effect in CurrentPartyMember.activeEffects)
        {
            GameObject iconObj = Instantiate(StatusIconPrefab, StatusEffectContainer);
            StatusIconUI icon = iconObj.GetComponent<StatusIconUI>();
            icon.Setup(effect);
            ActiveIcons[effect] = iconObj;
        }
    }

    public void SpawnCritText()
    {
        Instantiate(CritText, this.transform);
    }
    public void SpawnDodgeText()
    {
        Instantiate(DodgeText, this.transform);
    }

}
