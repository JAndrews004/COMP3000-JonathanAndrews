using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class MediumAI : EnemyAI
{
    public CombatMember Enemy;
    public override Turn ChooseAction(EnemyMember enemy)
    {
        Enemy = enemy;
        Debug.Log(enemy.baseStats.characterName + " is choosing action");

        Turn turn = new Turn(new List<CombatMember>(), null, null);
        List<CombatMember> validTargets = new List<CombatMember>();
        List<Ability> usableAbilities = new List<Ability>();
        foreach (Ability ability in enemy.activeAbilities)
        {
            if (ability.cooldownLeft == 0 && ability.usesLeft > 0)
            {
                usableAbilities.Add(ability);
            }
        }

        if (usableAbilities.Count == 0)
        {
            return turn;
        }

        Ability bestAbility = null;
        int bestScore = -9999999;

        int score = 0;
        foreach(Ability ability in usableAbilities)
        {
            score = EvaluateAbility(ability.AbilityData);
            if(score > bestScore)
            {
                bestAbility = ability;
                bestScore = score;
            }
        }
        Ability chosenAbility = bestAbility;


        if (chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy || chosenAbility.AbilityData.targetType == AbilityData.TargetType.MultipleEnemy || chosenAbility.AbilityData.targetType == AbilityData.TargetType.AllEnemies)
        {
            validTargets = GetAlivePlayers();
        }
        else if (chosenAbility.AbilityData.targetType == AbilityData.TargetType.DeadAlly)
        {
            foreach (CombatMember target in GameManager.Instance.EnemyMembers)
            {
                if (!target.Alive)
                {
                    validTargets.Add(target);
                }
            }
        }
        else
        {
            validTargets = GetAliveEnemies();

        }

        if (chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy || chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleAlly)
        {
            if (validTargets.Count > 0)
            {
                CombatMember target = ChooseBestTarget(validTargets,chosenAbility.AbilityData);
                turn = new Turn(new List<CombatMember> { target }, chosenAbility, enemy);
            }

        }
        else
        {
            List<CombatMember> targets = new List<CombatMember>();
            if (chosenAbility.AbilityData.numberOfTargets > validTargets.Count)
            {
                targets = validTargets;
            }
            else
            {
                for (int i = 0; i < chosenAbility.AbilityData.numberOfTargets; i++)
                {
                    CombatMember target = ChooseBestTarget(validTargets, chosenAbility.AbilityData);
                    targets.Add(target);
                    validTargets.Remove(target);
                    if (validTargets.Count == 0)
                    {
                        break;
                    }
                }
            }
            turn = new Turn(targets, chosenAbility, enemy);
        }
        Debug.Log(enemy.baseStats.characterName + " has selected an action");
        return turn;
    }

    public int EvaluateAbility(AbilityData ability)
    {
        
         int score = 0;


        foreach(AbilityTag tag in ability.tags)
        {

        
                switch (tag)
                {
                    case AbilityTag.Damage:
                        score += 20;
                        break;


                    case AbilityTag.Heal:
                        {
                            bool lowHealth = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if ((float)ally.CurrentHealth / (float)ally.CurrentMaxHealth <= 0.4f)
                                {
                                    lowHealth = true;
                                    break;
                                }
                            }

                            score += lowHealth ? 35 : 5;
                            break;
                        }


                    case AbilityTag.Regen:
                        {
                            bool lowHealth = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if ((float)ally.CurrentHealth / (float)ally.CurrentMaxHealth <= 0.5f)
                                {
                                    lowHealth = true;
                                    break;
                                }
                            }

                            score += lowHealth ? 20 : 5;
                            break;
                        }


                    case AbilityTag.Stun:
                        score += 25;
                        break;


                    case AbilityTag.Sleep:
                        score += 22;
                        break;


                    case AbilityTag.Poison:
                    case AbilityTag.Bleed:
                        score += 15;
                        break;


                    case AbilityTag.Curse:
                        score += 16;
                        break;


                    case AbilityTag.Vulnerability:
                        score += 18;
                        break;


                    case AbilityTag.Buff:
                        score += 12;
                        break;


                    case AbilityTag.Debuff:
                        score += 15;
                        break;


                    case AbilityTag.Shield:
                    case AbilityTag.Guard:
                        {
                            bool lowHealth = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if ((float)ally.CurrentHealth / (float)ally.CurrentMaxHealth <= 0.5f)
                                {
                                    lowHealth = true;
                                    break;
                                }
                            }

                            score += lowHealth ? 20 : 10;
                            break;
                        }


                    case AbilityTag.Taunt:
                        {
                            bool allyLow = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if ((float)ally.CurrentHealth / (float)ally.CurrentMaxHealth <= 0.4f)
                                {
                                    allyLow = true;
                                    break;
                                }
                            }

                            score += allyLow ? 22 : 10;
                            break;
                        }


                    case AbilityTag.LifeSteal:
                        {
                            bool enemyLow = false;

                            foreach (CombatMember enemy in GetAlivePlayers())
                            {
                                if ((float)enemy.CurrentHealth / (float)enemy.CurrentMaxHealth <= 0.5f)
                                {
                                    enemyLow = true;
                                    break;
                                }
                            }

                            score += enemyLow ? 28 : 18;
                            break;
                            
                        }


                    case AbilityTag.Cleanse:
                        {
                            bool debuffed = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if (ally.HasDebuff())
                                {
                                    debuffed = true;
                                    break;
                                }
                            }

                            score += debuffed ? 25 : 5;
                            break;
                        }


                    case AbilityTag.Unstun:
                        {
                            bool stunned = false;

                            foreach (CombatMember ally in GetAliveEnemies())
                            {
                                if (ally.IsStunned)
                                {
                                    stunned = true;
                                    break;
                                }
                            }

                            score += stunned ? 30 : 5;
                            break;
                        }


                    case AbilityTag.Dispel:
                        score += 15;
                        break;


                    case AbilityTag.Reflect:
                        score += 15;
                        break;


                    case AbilityTag.Immunity:
                        score += 14;
                        break;

                }
            
        }
        if (ability.numberOfTargets > 1)
        {
            score += (ability.numberOfTargets - 1) * 5;
        }
        score += Random.Range(0, 3);

        return score;
    }

    public CombatMember ChooseBestTarget(List<CombatMember> targets, AbilityData ability)
    {
        CombatMember currentTarget = null;
        foreach (AbilityTag tag in ability.tags)
        {
            switch (tag)
            {
                case AbilityTag.Damage:
                case AbilityTag.LifeSteal:
                case AbilityTag.Poison:
                case AbilityTag.Bleed:
                    
                    foreach (CombatMember target in targets)
                    {
                        if (currentTarget == null)
                        {
                            currentTarget = target;
                        }
                        else
                        {
                            if (currentTarget.CurrentHealth > target.CurrentHealth)
                            {
                                currentTarget = target;
                            }
                        }
                    }
                    break;
                case AbilityTag.Shield:
                case AbilityTag.Guard:
                case AbilityTag.Regen:
                case AbilityTag.Heal:
                    currentTarget = null;
                    foreach (CombatMember target in targets)
                    {
                        if (currentTarget == null)
                        {
                            currentTarget = target;
                        }
                        else
                        {
                            if (currentTarget.CurrentHealth > target.CurrentHealth && target == Enemy)
                            {
                                
                                
                                    currentTarget = target;
                            }
                        }
                    }
                    break;
                
                case AbilityTag.Buff:
                case AbilityTag.Immunity:
                    currentTarget = null;
                    foreach (CombatMember target in targets)
                    {
                        if (currentTarget == null)
                        {
                            currentTarget = target;
                        }
                        else
                        {
                            if (currentTarget.CurrentAttack + currentTarget.CurrentIntelligence > target.CurrentAttack + target.CurrentIntelligence)
                            {
                                currentTarget = target;
                            }
                        }
                    }
                    break;
                case AbilityTag.Stun:
                case AbilityTag.Debuff:
                case AbilityTag.Sleep:
                case AbilityTag.Curse:
                case AbilityTag.Vulnerability:
                case AbilityTag.Taunt:
                    currentTarget = null;
                    foreach (CombatMember target in targets)
                    {
                        if (currentTarget == null)
                        {
                            currentTarget = target;
                        }
                        else
                        {
                            if (currentTarget.CurrentHealth < target.CurrentHealth)
                            {
                                currentTarget = target;
                            }
                        }
                    }
                    break;
                case AbilityTag.Cleanse:
                    currentTarget = null;
                    int currentMaxDebuffs = 0;
                    int currentDebuffs = 0;
                    foreach (CombatMember target in targets)
                    {
                        if(currentTarget == null)
                        {
                            currentTarget = target;

                            foreach(Effect effect in target.activeEffects)
                            {
                                if(effect.statusEffectType == StatusEffect.Debuff)
                                {
                                    currentMaxDebuffs++;
                                }
                            }
                        }
                        else
                        {
                            foreach (Effect effect in target.activeEffects)
                            {
                                if (effect.statusEffectType == StatusEffect.Debuff)
                                {
                                    currentDebuffs++;
                                }
                            }

                            if (currentDebuffs > currentMaxDebuffs)
                            {
                                currentTarget = target;
                                currentMaxDebuffs = currentDebuffs;
                            }
                        }

                    }
                    break;
                case AbilityTag.Unstun:
                    currentTarget = null;
                    
                    foreach (CombatMember target in targets)
                    {
                        if (target.IsStunned)
                        {
                            currentTarget = target;
                        }

                    }
                    break;
                case AbilityTag.Dispel:
                    currentTarget = null;
                    int currentMaxBuffs = 0;
                    int currentBuffs = 0;
                    foreach (CombatMember target in targets)
                    {
                        if (currentTarget == null)
                        {
                            currentTarget = target;

                            foreach (Effect effect in target.activeEffects)
                            {
                                if (effect.statusEffectType == StatusEffect.Buff)
                                {
                                    currentMaxBuffs++;
                                }
                            }
                        }
                        else
                        {
                            foreach (Effect effect in target.activeEffects)
                            {
                                if (effect.statusEffectType == StatusEffect.Buff)
                                {
                                    currentBuffs++;
                                }
                            }

                            if (currentBuffs > currentMaxBuffs)
                            {
                                currentTarget = target;
                                currentMaxDebuffs = currentBuffs;
                            }
                        }

                    }
                    break;
                

            }
            
            if(currentTarget == null)
            {
                currentTarget = targets[Random.Range(0, targets.Count)];
            }
           
        } 
        return currentTarget;
    }
}
