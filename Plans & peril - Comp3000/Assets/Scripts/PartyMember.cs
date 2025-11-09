using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatMember
{
    public Characters baseStats;

    public int level;
    public int Xp;
    public int XpToLevelUp;
    public bool HasTurn { get; set; } = true;
   

   
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

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
        CurrentMaxHealth = baseStats.maxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentAttack = baseStats.attack;
        CurrentDefense = baseStats.defense;
        CurrentIntelligence = baseStats.intelligence;

        UpdateStats();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStats()
    {

    }

    
   
}
class ActiveEffect
{
    AbilityData source;
    StatType statModified;
    float modifier;
    int remainingTurns;
}
    
