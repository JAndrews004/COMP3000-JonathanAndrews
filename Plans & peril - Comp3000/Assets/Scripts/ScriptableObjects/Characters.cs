using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character")]
public class Characters : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public Sprite characterSprite;
    public int level;
    public int xp;

    [Header("Stats")]
    public int maxHealth;
    public int attack;
    public int defense;
    public int intelligence;
    public int magicDefence;
    public int Luck;
    public int avaliableStatPoints;

    [Header("Skills")]
    public List<AbilityData> unlockableAbilities;
    public List<AbilityData> equippedAbilities;

    [Header("Element")]
    public Element element;
    public AbilityData EquippedElement;

    [Header("Extras")]
    public RuntimeAnimatorController controller;
    public Sprite HeadShot;
}

public enum Element
{
    None,
    Fire,
    Water,
    Earth,
    Air,
}