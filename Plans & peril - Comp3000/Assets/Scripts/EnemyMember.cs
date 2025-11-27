using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyMember : CombatMember
{

    public EnemyAI aiController;

    public int Level;
    public int XPGiven;
    
    Tier tier;


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

        if(aiController is EasyAI)
        {
            tier = Tier.Easy;
            XPGiven = Mathf.RoundToInt(10 * (Level * Level) * 0.8f);
        }
        /*
        if (aiController is MediumAI)
        {
            tier = Tier.Medium;
        XPGiven = Mathf.RoundToInt(10 * (Level * Level));
        }
        if (aiController is HardAI)
        {
            tier = Tier.Hard;
        XPGiven = Mathf.RoundToInt(10 * (Level * Level) * 1.5f);
        }
        if (aiController is BossAI)
        {
            tier = Tier.Boss;
        XPGiven = Mathf.RoundToInt(10 * (Level * Level) * 3);
        }
        */
        XPGiven = Mathf.RoundToInt(XPGiven*Random.Range(0.95f, 1.05f));
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

  

        UpdateStats(Level,tier,GameManager.Instance.EnemyMembers.Count);
    }


    void UpdateStats(int Level, Tier tier ,int numOfEnemies)
    {
        float k_hp = 0.06f;
        float hp_exp = 1.02f;
        float k_atk = 0.045f;
        float k_def = 0.035f;
        float k_int = 0.04f;
        float k_mdef = 0.03f;

        CurrentMaxHealth = Mathf.RoundToInt(baseStats.maxHealth * Mathf.Pow(1 + Level * k_hp, hp_exp));
        CurrentAttack = Mathf.RoundToInt(baseStats.attack * (1 + Level * k_atk));
        CurrentDefense = Mathf.RoundToInt(baseStats.defense * (1 + Level * k_def));
        CurrentIntelligence = Mathf.RoundToInt(baseStats.intelligence * (1 + Level * k_int));
        CurrentMagicDefense = Mathf.RoundToInt(baseStats.magicDefence * (1 + Level * k_mdef));

        
        CurrentLuck = Mathf.RoundToInt(baseStats.Luck + Level * 0.6f);
        float tierMult = 0.95f;
        switch (tier){
            case Tier.Easy:
                tierMult = 0.95f;
                break;
            case Tier.Medium:
                tierMult = 1f;
                break;
            case Tier.Hard:
                tierMult = 1.35f;
                break;
            case Tier.Boss:
                tierMult = 3.0f;
                break;
        }

        float allyPenalty = 1 - Mathf.Clamp((numOfEnemies - 1) * 0.08f, 0, 0.6f);

        CurrentMaxHealth *= Mathf.RoundToInt(tierMult * allyPenalty);
        CurrentAttack *= Mathf.RoundToInt(tierMult * allyPenalty);
        CurrentDefense *= Mathf.RoundToInt(tierMult * allyPenalty);
        CurrentIntelligence *= Mathf.RoundToInt(tierMult * allyPenalty);
        CurrentMagicDefense *= Mathf.RoundToInt(tierMult * allyPenalty);

        var randomRange = Random.Range(-0.08f, 0.08f);
        CurrentMaxHealth *= Mathf.RoundToInt(1 + randomRange);
        CurrentAttack *= Mathf.RoundToInt(1 + randomRange);
        CurrentIntelligence *= Mathf.RoundToInt(1 + randomRange);

        CurrentMaxHealth = Mathf.RoundToInt(Mathf.Clamp(CurrentMaxHealth,baseStats.maxHealth*0.6f, baseStats.maxHealth*10));
        CurrentAttack = Mathf.RoundToInt(Mathf.Clamp(CurrentAttack, baseStats.attack * 0.5f, baseStats.attack * 6));
        CurrentIntelligence = Mathf.RoundToInt(Mathf.Clamp(CurrentIntelligence, baseStats.intelligence * 0.5f, baseStats.intelligence * 6));

        CurrentHealth = CurrentMaxHealth;
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
            string baseMessage ="";
            bool taunted = false;
            Ability ability = action.Action;
            List<CombatMember> target = action.Target;

            if (ability.AbilityData.IsTauntable)
            {
                List<CombatMember> newTargets = new List<CombatMember>();
                List<CombatMember> Taunters = new List<CombatMember>();
                
                foreach (Effect effect in activeEffects)
                {
                    if (effect is TauntEffect)
                    {
                        taunted = true;
                        Taunters.Add(effect.User);
                    }
                }
                foreach (CombatMember t in Taunters)
                {
                    if (newTargets.Count == target.Count)
                    {
                        break;
                    }
                    newTargets.Add(t);
                }

                foreach (CombatMember t in target)
                {
                    if (newTargets.Count == target.Count)
                    {
                        break;
                    }
                    if (!newTargets.Contains(t))
                    {
                        newTargets.Add(t);
                    }
                }
                baseMessage = $"<color=#FF0000>{action.Attacker.baseStats.characterName}</color> was prevoked by {string.Join(", ", Taunters.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"))}";
                target = newTargets;
            }
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

            
            
            action.Action.DecreaseUses(this);
            action.Action.cooldownLeft = action.Action.AbilityData.cooldown;
            string targetNames = string.Join(", ", action.Target.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"));

            if (action.Target != null)
            {
                if (action.Target[0] is EnemyMember)
                {
                    targetNames = string.Join(", ", action.Target.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"));
                }
            }
            if (!taunted)
            {
                combatManager.battleLogManager.AddMessage(
                $"<color=#FF0000>{action.Attacker.baseStats.characterName}</color> " +
                $"used <color=#0000FF>{action.Action.AbilityData.abilityName}</color> on {targetNames}"
                );
            }
            else
            {
                combatManager.battleLogManager.AddMessage($"{baseMessage} and used <color=#0000FF>{action.Action.AbilityData.abilityName}</color>");
            }
            




        }
        yield return new WaitForSeconds(Random.Range(1,4));
    }
    public enum Tier
    {
        Easy,
        Medium,
        Hard, 
        Boss
    }
}
