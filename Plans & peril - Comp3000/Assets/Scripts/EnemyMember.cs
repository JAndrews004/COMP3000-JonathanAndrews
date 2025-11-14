using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyMember : CombatMember
{

    public EnemyAI aiController;

    public int Level;
    public int XPGiven;
    


    private void Awake()
    {
        // Prevent duplicates when reloading scenes
        if (FindObjectsOfType<EnemyMember>().Length > 6) 
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

    public IEnumerator TakeTurn()
    {
        if (!Alive)
        {
            yield break;
        }
        yield return ExecuteAction(aiController.ChooseAction(this));
        //yield until action is complete
    }

    public IEnumerator ExecuteAction(Turn action)
    {
        if (action.Action != null && action.Target != null && action.Attacker != null && action!=null)
        {
            Ability ability = action.Action;
            List<CombatMember> target = action.Target;

            if (ability.usesLeft > 0 && ability.cooldownLeft == 0)
            {
                if (ability.AbilityData.PhysicalBehaviour != null)
                {
                    ability.AbilityData.PhysicalBehaviour.Execute(this, target, ability.AbilityData);
                }
                if (ability.AbilityData.EffectBehaviour != null)
                {
                    ability.AbilityData.EffectBehaviour.Execute(this, target, ability.AbilityData);
                }
            }

            
            
            action.Action.DecreaseUses();
            action.Action.cooldownLeft = action.Action.AbilityData.cooldown;
            string targetNames = string.Join(", ", action.Target.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"));

            if (action.Target != null)
            {
                if (action.Target[0] is EnemyMember)
                {
                    targetNames = string.Join(", ", action.Target.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"));
                }
            }

            combatManager.battleLogManager.AddMessage(
                $"<color=#FF0000>{action.Attacker.baseStats.characterName}</color> " +
                $"used <color=#0000FF>{action.Action.AbilityData.abilityName}</color> on {targetNames}"
            );




        }
        yield return new WaitForSeconds(Random.Range(2,4));
    }
    
}
