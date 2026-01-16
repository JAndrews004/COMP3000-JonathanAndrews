using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySlot : MonoBehaviour
{
    public EnemyMember CurrentEnemyMember;
    public TextMeshPro NameText;
    public GameObject TargetHighlight;
    public Slider HPBar;
    public Slider ShieldBar;
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
        if (CurrentEnemyMember != null)
        {
            NameText.text = CurrentEnemyMember.baseStats.characterName;
            HPBar.maxValue = CurrentEnemyMember.CurrentMaxHealth;
            HPBar.value = CurrentEnemyMember.CurrentHealth;
            ShieldBar.maxValue = CurrentEnemyMember.CurrentMaxHealth;
            ShieldBar.value = CurrentEnemyMember.shieldValue;

            if (!gameObject.GetComponent<Animator>() || CurrentEnemyMember.baseStats.controller == null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentEnemyMember.baseStats.characterSprite;

            }
            if (!gameObject.GetComponent<Animator>().runtimeAnimatorController)
            {
                gameObject.GetComponent<Animator>().runtimeAnimatorController = CurrentEnemyMember.baseStats.controller;
                gameObject.GetComponent<Animator>().speed = Random.RandomRange(0.25f, 0.3f);
            }
        }
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
        if (CurrentEnemyMember == null) return;

        // Clear existing icons
        foreach (Transform child in StatusEffectContainer)
            Destroy(child.gameObject);
        ActiveIcons.Clear();

        // Rebuild list
        foreach (Effect effect in CurrentEnemyMember.activeEffects)
        {
            GameObject iconObj = Instantiate(StatusIconPrefab, StatusEffectContainer);
            StatusIconUI icon = iconObj.GetComponent<StatusIconUI>();
            icon.Setup(effect);
            ActiveIcons[effect] = iconObj;
        }
    }
}
