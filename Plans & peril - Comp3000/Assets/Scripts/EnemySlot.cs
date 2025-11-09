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
        if (CurrentEnemyMember != null)
        {
            NameText.text = CurrentEnemyMember.baseStats.characterName;
            HPBar.maxValue = CurrentEnemyMember.CurrentMaxHealth;
            HPBar.value = CurrentEnemyMember.CurrentHealth;
            ShieldBar.maxValue = CurrentEnemyMember.CurrentMaxHealth;
            ShieldBar.value = CurrentEnemyMember.shieldValue;

            if (!HasAssignedSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentEnemyMember.baseStats.characterSprite;
                HasAssignedSprite = true;
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
}
