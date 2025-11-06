using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyMember : CombatMember
{

    public Enemy baseStats;
    public EnemyAI aiController;

    public int Level;
    public int XPGiven;
    public bool Alive => CurrentHealth > 0;


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


    void UpdateStats(int Level)
    {

    }

    public void BasicAttack(PartyMember target) 
    {
        target.TakeDamage(CurrentAttack);
    
    }

    public IEnumerator TakeTurn()
    {
        if (!Alive)
        {
            yield break;
        }
        yield return ExecuteAction(aiController.ChooseAction(this));
        //yield until action is complete
    }

    public IEnumerator ExecuteAction(List<Turn> actions)
    {
        foreach (Turn action in actions)
        {
            Ability ability = action.Action;
            List<CombatMember> target = action.Target;

            if (ability.usesLeft > 0 && ability.cooldownLeft == 0)
            {
                ability.AbilityData.behaviour.Execute(this, target);
            }
        }
        if (actions.Count >=1 && actions[0] != null)
        {
            actions[0].Action.DecreaseUses();
            actions[0].Action.cooldownLeft = actions[0].Action.AbilityData.cooldown;
        }
        Debug.Log(baseStats.characterName + " has executed an action");
        yield return null;
    }
    
}
