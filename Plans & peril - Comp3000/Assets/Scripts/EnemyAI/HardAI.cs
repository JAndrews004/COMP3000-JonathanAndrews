using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardAI : EnemyAI
{
    public CombatMember Enemy;
    public List<CombatMember> players;
    public List<CombatMember> enemies;
    public override Turn ChooseAction(EnemyMember enemy)
    {
        Enemy = enemy;

        players = GetAlivePlayers();
        enemies = GetAliveEnemies();

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
        CombatMember bestTarget = null;
        int bestScore = -9999999;

        foreach (Ability ability in usableAbilities)
        {
            if (ability.AbilityData.targetType == AbilityData.TargetType.SingleEnemy || ability.AbilityData.targetType == AbilityData.TargetType.MultipleEnemy || ability.AbilityData.targetType == AbilityData.TargetType.AllEnemies)
            {
                validTargets = players;
            }
            else if(ability.AbilityData.targetType == AbilityData.TargetType.DeadAlly)
            {
                foreach(CombatMember target in GameManager.Instance.EnemyMembers)
                {
                    if (!target.Alive)
                    {
                        validTargets.Add(target);
                    }
                }
            }
            else
            {
                validTargets = enemies;

            }
            

            foreach (CombatMember target in validTargets)
            {
                int score = EvaluateAbility(ability.AbilityData);

                score += EvaluateTarget(target, ability.AbilityData);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAbility = ability;
                    bestTarget = target;
                }
            }
        }
        Ability chosenAbility = bestAbility;
        CombatMember chosenTarget = bestTarget;

        if (chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy ||
        chosenAbility.AbilityData.targetType == AbilityData.TargetType.MultipleEnemy ||
        chosenAbility.AbilityData.targetType == AbilityData.TargetType.AllEnemies)
        {
            validTargets = players;
        }
        else
        {
            validTargets = enemies;
        }

        if (chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy ||
        chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleAlly)
        {
            turn = new Turn(new List<CombatMember> { chosenTarget }, chosenAbility, enemy);
        }

        else
        {
            List<CombatMember> targets = new List<CombatMember>();

            targets.Add(chosenTarget);
            validTargets.Remove(chosenTarget);

            for (int i = 1; i < chosenAbility.AbilityData.numberOfTargets; i++)
            {
                if (validTargets.Count == 0)
                    break;

                CombatMember nextTarget = validTargets[Random.Range(0, validTargets.Count)];

                targets.Add(nextTarget);
                validTargets.Remove(nextTarget);
            }

            turn = new Turn(targets, chosenAbility, enemy);
        }


        Debug.Log(enemy.baseStats.characterName + " has selected an action");
        return turn;
    }

    public int EvaluateAbility(AbilityData ability)
    {

        int score = 0;


        foreach (AbilityTag tag in ability.tags)
        {


            switch (tag)
            {
                case AbilityTag.Damage:
                    score += 20;

                    float lowestHPPercentage = 1.0f;
                    int estimatedDamage = 0;

                    if (ability.powerType == AbilityPowerType.Physical)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Enemy.CurrentAttack * ability.PhysicalBehaviour.baseDamage;
                    }
                    else if (ability.powerType == AbilityPowerType.Magical)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Enemy.CurrentIntelligence * ability.PhysicalBehaviour.baseDamage;
                    }
                    else if (ability.powerType == AbilityPowerType.Mixed)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Mathf.RoundToInt(Enemy.CurrentIntelligence * 0.3f + Enemy.CurrentAttack * 0.7f) * ability.PhysicalBehaviour.baseDamage;
                    }
                    foreach (CombatMember enemy in players)
                    {
                        if ((float)enemy.CurrentHealth / (float)enemy.CurrentMaxHealth < lowestHPPercentage)
                        {
                            lowestHPPercentage = (float)enemy.CurrentHealth / (float)enemy.CurrentMaxHealth;
                        }
                        if (enemy.CurrentHealth < estimatedDamage)
                        {
                            score += 40;
                        }
                    }
                    if (lowestHPPercentage <= 0.35f)
                    {
                        score += 15;
                    }

                    

                    break;


                case AbilityTag.Heal:
                    {
                        bool lowHealth = false;

                        foreach (CombatMember ally in enemies)
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

                        foreach (CombatMember ally in enemies)
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

                        foreach (CombatMember ally in enemies)
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

                        foreach (CombatMember ally in enemies)
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

                        foreach (CombatMember enemy in players)
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

                        foreach (CombatMember ally in enemies)
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

                        foreach (CombatMember ally in enemies)
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

                case AbilityTag.Blowback:
                    score -= 10;
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

    public int EvaluateTarget(CombatMember target, AbilityData ability)
    {
        float score = 0;
        float targetHPPercent = (float)target.CurrentHealth / (float)target.CurrentMaxHealth;
        foreach (AbilityTag tag in ability.tags)
        {
            switch (tag)
            {
                case AbilityTag.Damage:
                case AbilityTag.LifeSteal:
                case AbilityTag.Poison:
                case AbilityTag.Bleed:

                    
                    int estimatedDamage = 0;

                    if (ability.powerType == AbilityPowerType.Physical)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Enemy.CurrentAttack * ability.PhysicalBehaviour.baseDamage;
                    }
                    else if (ability.powerType == AbilityPowerType.Magical)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Enemy.CurrentIntelligence * ability.PhysicalBehaviour.baseDamage;
                    }
                    else if (ability.powerType == AbilityPowerType.Mixed)
                    {
                        if (ability.PhysicalBehaviour)
                            estimatedDamage = Mathf.RoundToInt(Enemy.CurrentIntelligence * 0.3f + Enemy.CurrentAttack * 0.7f) * ability.PhysicalBehaviour.baseDamage;
                    }


                    if (target.CurrentHealth < estimatedDamage)
                    {
                        score += 40;
                    }
                    
                    score += (1 - targetHPPercent) * 20;

                    

                    break;
                case AbilityTag.Shield:
                case AbilityTag.Guard:
                case AbilityTag.Regen:
                case AbilityTag.Heal:
                    score += (1 - targetHPPercent) * 30;
                    if(target == Enemy)
                    {
                        score -= 9999999999;
                    }
                    break;

                case AbilityTag.Buff:
                case AbilityTag.Immunity:
                    score += target.CurrentAttack + target.CurrentIntelligence;
                    break;
                case AbilityTag.Stun:
                case AbilityTag.Debuff:
                case AbilityTag.Sleep:
                case AbilityTag.Curse:
                case AbilityTag.Vulnerability:
                case AbilityTag.Taunt:
                    score += target.CurrentAttack + target.CurrentIntelligence;
                    break;
                case AbilityTag.Cleanse:
               
                    int currentDebuffs = 0;

                    foreach (Effect effect in target.activeEffects)
                    {
                        if (effect.statusEffectType == StatusEffect.Debuff)
                        {
                            currentDebuffs++;
                        }
                    }

                    score += currentDebuffs * 15;

                    break;
                case AbilityTag.Unstun:
                    if (target.IsStunned)
                    {
                        score += 20;
                    }
                    break;
                case AbilityTag.Dispel:

                    int currentBuffs = 0;
                    foreach (Effect effect in target.activeEffects)
                    {
                        if (effect.statusEffectType == StatusEffect.Buff)
                        {
                            currentBuffs++;
                        }
                        
                    }
                    score += currentBuffs * 15;
                    break;


            }

            

        }
        int Score = Mathf.RoundToInt(score);
        return Score;
    }
}
