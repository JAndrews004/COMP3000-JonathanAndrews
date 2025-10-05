using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMember : MonoBehaviour
{

    public Enemy baseStats;

    public int CurrentMaxHealth;
    public int CurrentHealth;
    public int CurrentAttack;
    public int CurrentDefense;
    public int CurrentIntelligence;
    public int Level;
    public int XPGiven;
    public bool Alive => CurrentHealth > 0;

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
    public void TakeDamage(int AttackPower)
    {
        if (CurrentHealth - AttackPower <= 0)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= AttackPower;
        }
    }
}
