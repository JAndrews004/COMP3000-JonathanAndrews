using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EasyAI : EnemyAI
{
    public override List<Turn> ChooseAction(EnemyMember enemy)
    {
        Debug.Log(enemy.baseStats.characterName + " is choosing action");
        List<Turn> turns = new List<Turn>();
        List<CombatMember> validTargets = new List<CombatMember>();
        List<Ability> usableAbilities = new List<Ability>();
        foreach (Ability ability in enemy.abilities)
        {
            if(ability.cooldownLeft == 0 && ability.usesLeft > 0)
            {
                usableAbilities.Add(ability);
            }
        }

        if (usableAbilities.Count == 0)
        {
            return turns;
        }

        Ability chosenAbility = usableAbilities[Random.Range(0,usableAbilities.Count)];

        if(chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy|| chosenAbility.AbilityData.targetType == AbilityData.TargetType.MultipleEnemy|| chosenAbility.AbilityData.targetType == AbilityData.TargetType.AllEnemies)
        {
            validTargets = GetAlivePlayers();
        }
        else
        {
            validTargets = GetAliveEnemies();

        }

        if(chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleEnemy || chosenAbility.AbilityData.targetType == AbilityData.TargetType.SingleAlly)
        {
            CombatMember target = validTargets[Random.Range(0,validTargets.Count)];
            turns.Add(new Turn(new List<CombatMember> { target }, chosenAbility,enemy));
        }
        else
        {
            List<CombatMember> targets = new List<CombatMember>();
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
            turns.Add(new Turn(targets, chosenAbility, enemy));
        }
        Debug.Log(enemy.baseStats.characterName + " has selected an action");
        return turns;
    }
}
