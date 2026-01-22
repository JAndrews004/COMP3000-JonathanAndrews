using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager.Requests;
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

    public List<AbilityData> ALLUNLOCKABLEABILITIES;
    public CharacterSkillTree characterSkillTree;
    
    
    private void Awake()
    {
        ALLUNLOCKABLEABILITIES = baseStats.unlockableAbilities;
        abilityDatas = baseStats.equippedAbilities;
       
        foreach (AbilityData ability in ALLUNLOCKABLEABILITIES)
        {
            if (ability.unlocked)
            {
                characterSkillTree.unlockedAbilities.Add(ability);
            }
        }
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
        availableStatPoints = baseStats.avaliableStatPoints;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

        UpdateStats();

        UpdateXpRequired();

    }

    // Update is called once per frame
    public void Update()
    {
        if(combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    float a = Alive ? 1f : 0.5f;
                    GameManager.Instance.fXManager.SetAlpha(slot, a);
                    break;

                }
            }
        }
        baseStats.equippedAbilities = abilityDatas;
        
    }
    public override void SpawnBuffEffect()
    {
        if (combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    GameManager.Instance.fXManager.SpawnBuffEffect(slot.transform,false);
                    break;

                }
            }
        }
    }
    public override void SpawnDebuffEffect()
    {
        if (combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    GameManager.Instance.fXManager.SpawnDebuffEffect(slot.transform, false);
                    break;

                }
            }
        }
    }
    public override void SpawnHealEffect()
    {
        if (combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    GameManager.Instance.fXManager.spawnHealEffect(slot.transform, false);
                    break;
                }
            }
        }
    }
    public override void SpawnReviveEffect()
    {
        if (combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    GameManager.Instance.fXManager.spawnReviveEffect(slot.transform, false);
                    break;
                }
            }
        }
    }
    public override void SpawnStunEffect()
    {
        if (combatManager != null)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == this)
                {
                    GameManager.Instance.fXManager.spawnStunEffect(slot.transform, false);
                    break;
                }
            }
        }
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
        baseStats.avaliableStatPoints = availableStatPoints;
    }

    public bool equipSkill(AbilityData ability, int slot)
    {
        if (abilityDatas.Contains(ability) && abilityDatas[slot - 1] == null && ability.abilityCategory != AbilityData.AbilityCategory.Passive)
        {
            
            int index = abilityDatas.IndexOf(ability);
            abilityDatas[slot - 1] = ability;
            abilityDatas[index] = null;

        }
        else if (abilityDatas.Contains(ability) && abilityDatas[slot - 1] == null && ability.abilityCategory == AbilityData.AbilityCategory.Passive)
        {
            if(slot >= 5)
            {
                int index = abilityDatas.IndexOf(ability);
                abilityDatas[slot - 1] = ability;
                abilityDatas[index] = null;
            }
        }
        else if (slot <=4 && ability.abilityCategory != AbilityData.AbilityCategory.Passive)
        {
            if (abilityDatas.Contains(ability))
            {
                
                int index = abilityDatas.IndexOf(ability);
                if(index < 4){
                    AbilityData temp = abilityDatas[slot-1];
                    abilityDatas[slot - 1] = ability;
                    abilityDatas[index] = temp;
                }
                else if (ability.abilityCategory != AbilityData.AbilityCategory.Passive && abilityDatas[slot - 1].abilityCategory != AbilityData.AbilityCategory.Passive)
                {
                    AbilityData temp = abilityDatas[slot - 1];
                    abilityDatas[slot - 1] = ability;
                    abilityDatas[index] = temp;
                }
                

            }
            else
            {
                abilityDatas[slot-1] = ability;
            }
            
            return true;
        }
        else if (slot<=6)
        {
            
            if (abilityDatas.Contains(ability))
            {

                int index = abilityDatas.IndexOf(ability);
                if(index <4 && ability.abilityCategory != AbilityData.AbilityCategory.Passive && abilityDatas[slot - 1].abilityCategory != AbilityData.AbilityCategory.Passive)
                {
                    AbilityData temp = abilityDatas[slot - 1];
                    abilityDatas[slot - 1] = ability;
                    abilityDatas[index] = temp;
                }
                else if(index >=4 && slot > 4)
                {
                    AbilityData temp = abilityDatas[slot - 1];
                    abilityDatas[slot - 1] = ability;
                    abilityDatas[index] = temp;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                abilityDatas[slot - 1] = ability;
            }
            
            return true;
        }

        return false;
    }

    public void EquipElement(AbilityData Elmnt)
    {
        if (Elmnt.isElement)
        {
            Debug.Log("equipping element");
            element = Elmnt.elementTag;
            baseStats.element = Elmnt.elementTag;
            baseStats.EquippedElement = Elmnt;
        }
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
