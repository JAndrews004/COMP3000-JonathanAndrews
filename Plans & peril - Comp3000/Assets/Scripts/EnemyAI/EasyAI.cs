using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EasyAI : EnemyAI
{
    bool guard = false;
    public override Turn ChooseAction(EnemyMember enemy)
    {
        Debug.Log(enemy.baseStats.characterName + " is choosing action");

        Turn turn = new Turn(new List<CombatMember>(), null, null);
        List<CombatMember> validTargets = new List<CombatMember>();
        List<Ability> usableAbilities = new List<Ability>();
        foreach (Ability ability in enemy.activeAbilities)
        {
            if(ability.cooldownLeft == 0 && ability.usesLeft > 0)
            {
                usableAbilities.Add(ability);
            }
        }

        if (usableAbilities.Count == 0)
        {
            return turn;
        }

        Ability chosenAbility = usableAbilities[Random.Range(0,usableAbilities.Count)];
        foreach(AbilityTag tag in chosenAbility.AbilityData.tags)
        {
            if(tag == AbilityTag.Guard)
            {
                guard = true;
            }
        }

        if(chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy|| chosenAbility.AbilityData.targetType == AbilityData.TargetType.MultipleEnemy|| chosenAbility.AbilityData.targetType == AbilityData.TargetType.AllEnemies)
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
            if (guard && validTargets.Contains(enemy))
            {
                validTargets.Remove(enemy);
            }

        }

        if(chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy || chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleAlly)
        {
            if(validTargets.Count > 0)
            {
                CombatMember target = validTargets[Random.Range(0,validTargets.Count)];
                turn = new Turn(new List<CombatMember> { target }, chosenAbility,enemy);
            }
            
        }
        else
        {
            List<CombatMember> targets = new List<CombatMember>();
            if(chosenAbility.AbilityData.numberOfTargets > validTargets.Count)
            {
                targets = validTargets;
            }
            else {
                for(int i =0;i< chosenAbility.AbilityData.numberOfTargets; i++)
                {
                    CombatMember target = validTargets[Random.Range(0, validTargets.Count)];
                    targets.Add(target);
                    validTargets.Remove(target);
                    if(validTargets.Count == 0)
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
}
