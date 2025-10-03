using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour
{
    public Characters baseStats;

    public int CurrentMaxHealth;
    public int CurrentHealth;
    public int CurrentAttack;
    public int CurrentDefense;
    public int CurrentIntelligence;
    public int level;
    public int Xp;
    public int XpToLevelUp;

    private void Awake()
    {
        // Prevent duplicates when reloading scenes
        if (FindObjectsOfType<PartyMember>().Length > 4) // if you want exactly 4 members
        {
            Destroy(gameObject);
            return;
        }

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
