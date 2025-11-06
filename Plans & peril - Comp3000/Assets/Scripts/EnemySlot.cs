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
            if (!HasAssignedSprite)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = CurrentEnemyMember.baseStats.characterSprite;
                HasAssignedSprite = true;
            }
        }
    }
}
