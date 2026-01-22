using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAllocationViewModel
{
    
    public PartyMember character;
    public GameObject UiObject;

    public int DamangeIncrease = 0;
    public int DefenseIncrease = 0;
    public int IntelligenceIncrease = 0;
    public int MagicDefenseIncrease = 0;
    public int LuckIncrease = 0;
    public int HpIncrease = 0;


    public StatAllocationViewModel(PartyMember character, GameObject uiObject)
    {
        this.character = character;
        UiObject = uiObject;
    }
    public void ApplyStatPoints()
    {

        character.baseStats.attack += DamangeIncrease;
        character.baseStats.defense += DefenseIncrease;
        character.baseStats.intelligence += DamangeIncrease;
        character.baseStats.magicDefence += MagicDefenseIncrease;
        character.baseStats.Luck += LuckIncrease;
        character.baseStats.maxHealth += HpIncrease * 10;

        DamangeIncrease = 0;
        DefenseIncrease = 0;
        IntelligenceIncrease = 0;
        MagicDefenseIncrease = 0;
        LuckIncrease = 0;
        HpIncrease = 0;

    }
   
}
