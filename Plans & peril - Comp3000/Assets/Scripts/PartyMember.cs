using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatMember
{
    

    public int level;
    public int Xp;
    public int XpToLevelUp = 100;
    public bool HasTurn { get; set; } = true;

    public int availableStatPoints = 0;
    public event Action<PartyMember> levelUp;

    public CharacterClass Class;

    
    //List<ActiveEffect> activeEffectsTurns;
    private void Awake()
    {
        // Prevent duplicates when reloading scenes
        if (FindObjectsOfType<PartyMember>().Length > 4) // if you want exactly 4 members
        {
            Destroy(gameObject);
            return;
        }
        if (activeAbilities == null)
            activeAbilities = new List<Ability>();
        if (passiveAbilities == null)
            passiveAbilities = new List<Ability>();

        if (abilityDatas == null)
            abilityDatas = new List<AbilityData>();

        if (activeEffects == null)
            activeEffects = new List<Effect>();

        level = baseStats.level;
        Xp = baseStats.xp;
        if (level < 10)
        {
            Class = CharacterClass.F;
        }
        else if (level < 20)
        {
            Class = CharacterClass.E;
        }
        else if (level < 30)
        {
            Class = CharacterClass.D;
        }
        else if (level < 40)
        {
            Class = CharacterClass.C;
        }
        else if (level < 50)
        {
            Class = CharacterClass.B;
        }
        else if (level < 60)
        {
            Class = CharacterClass.A;
        }
        else if (level < 70)
        {
            Class = CharacterClass.S;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

        UpdateStats();

        UpdateXpRequired();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateStats()
    {
        CurrentMaxHealth = baseStats.maxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentAttack = baseStats.attack;
        CurrentDefense = baseStats.defense;
        CurrentIntelligence = baseStats.intelligence;
        CurrentLuck = baseStats.Luck;
        CurrentMagicDefense = baseStats.magicDefence;
    }

    void UpdateXpRequired()
    {
        XpToLevelUp = 100 * level * level;
    }
    public void AddXP(int xp)
    {
        Xp += xp;                
        UpdateXpRequired();    

        
        while (Xp >= XpToLevelUp)
        {
            Xp -= XpToLevelUp;   
            level++;

            //Give stat points based on new level
            if (level <= 10)
            {
                availableStatPoints += 3;
            }
            else if (level <= 20)
            {
                availableStatPoints += 2;
            }
            else if (level <= 40)
            {
                availableStatPoints += 4;
            }
            else
            {
                availableStatPoints += 2;
            }
            levelUp?.Invoke(this);
            UpdateXpRequired();  //Recalculate for next level
        }
        baseStats.level = level;
    }

    class ActiveEffect
    {
        AbilityData source;
        StatType statModified;
        float modifier;
        int remainingTurns;
    }
}
public enum CharacterClass
{
    S,
    A,
    B,
    C,
    D,
    E,
    F,
}
