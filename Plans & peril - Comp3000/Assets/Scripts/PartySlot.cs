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
    private void Start()
    { 
        //NameText = GetComponentInChildren<TextMeshPro>();
        if (TargetHighlight != null)
        {
            TargetHighlight.SetActive(false);
        }
        mat = GetComponent<SpriteRenderer>().material;
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

    public void RefreshStatusEffects()
    {
        if (CurrentPartyMember == null) return;

        // Clear existing icons
        foreach (Transform child in StatusEffectContainer)
            Destroy(child.gameObject);
        ActiveIcons.Clear();

        // Rebuild list
        foreach (Effect effect in CurrentPartyMember.activeEffects)
        {
            GameObject iconObj = Instantiate(StatusIconPrefab, StatusEffectContainer);
            StatusIconUI icon = iconObj.GetComponent<StatusIconUI>();
            icon.Setup(effect);
            ActiveIcons[effect] = iconObj;
        }
    }

}
