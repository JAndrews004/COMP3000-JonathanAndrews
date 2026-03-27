using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpViewModel
{
    public PartyMember character;
    public event Action<LevelUpViewModel> OnClosed;
    public GameObject UiObject;

    public int DamangeIncrease = 0;
    public int DefenseIncrease = 0;
    public int IntelligenceIncrease = 0;
    public int MagicDefenseIncrease = 0;
    public int LuckIncrease = 0;
    public int HpIncrease = 0;

    private float currentHPPercentage; 

    public LevelUpViewModel(PartyMember character, GameObject uiObject)
    {
        this.character = character;
        UiObject = uiObject;
        currentHPPercentage = character.CurrentHealth / character.CurrentMaxHealth;
    }
    public void ApplyStatPoints()
    {
      
        character.baseStats.attack += DamangeIncrease;
        character.baseStats.defense += DefenseIncrease;
        character.baseStats.intelligence += DamangeIncrease;
        character.baseStats.magicDefence += MagicDefenseIncrease;
        character.baseStats.Luck += LuckIncrease;
        character.baseStats.maxHealth += HpIncrease *10;

        DamangeIncrease = 0;
        DefenseIncrease = 0;
        IntelligenceIncrease = 0;
        MagicDefenseIncrease = 0;
        LuckIncrease = 0;
        HpIncrease = 0;

        character.UpdateStats();


    }
public void OnClosePressed()
    {
        OnClosed?.Invoke(this);
        character.baseStats.attack += 1;
        character.baseStats.defense += 1;
        character.baseStats.intelligence += 1;
        character.baseStats.magicDefence += 1;
        character.baseStats.Luck += 1;
        character.baseStats.maxHealth += 10;

        character.CurrentHealth = Mathf.RoundToInt(currentHPPercentage * character.baseStats.maxHealth);
        character.UpdateStats();
    }
}
