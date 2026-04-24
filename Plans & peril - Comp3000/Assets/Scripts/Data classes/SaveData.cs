using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public int money;
    public int passLevel;
    public bool tutorialCompleted;

    public List<CharacterSaveData> characters = new List<CharacterSaveData>();
    public List<string> unlockedAbilityIDs = new List<string>();
}

[System.Serializable]
public class CharacterSaveData
{
    public string characterID;

    public int level;
    public int xp;

    public int maxHealth;
    public int attack;
    public int defense;
    public int intelligence;
    public int magicDefence;
    public int luck;
    public int availableStatPoints;

    public List<string> equippedAbilityIDs = new List<string> { };

    public string equippedElementID;
}