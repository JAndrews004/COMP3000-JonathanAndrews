using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMember : CombatMember
{

    public Enemy baseStats;

    public int Level;
    public int XPGiven;
    public bool Alive => CurrentHealth > 0;

    public List<AbilityData> Abilities;
    //List<ActiveEffect> activeEffectsTurns;

    private void Awake()
    {
        // Prevent duplicates when reloading scenes
        if (FindObjectsOfType<EnemyMember>().Length > 6) // if you want exactly 4 members
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public EnemyMember(int level,int XP)
    {
        Level = level;
        XPGiven = XP;

        CurrentMaxHealth = baseStats.maxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentAttack = baseStats.attack;
        CurrentDefense = baseStats.defense;
        CurrentIntelligence = baseStats.intelligence;

  

        UpdateStats(Level);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStats(int Level)
    {

    }

    public void BasicAttack(PartyMember target) 
    {
        target.TakeDamage(CurrentAttack);
    
    }
   
    
}
