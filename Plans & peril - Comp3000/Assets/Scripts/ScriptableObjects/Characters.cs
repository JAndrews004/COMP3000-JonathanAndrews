using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character")]
public class Characters : ScriptableObject
{
    [Header("Basic Info")]
    public string characterName;
    public Sprite characterSprite;

    [Header("Stats")]
    public int maxHealth;
    public int attack;
    public int defense;
    public int intelligence;
    public int magicDefence;
    public int Luck;

}
