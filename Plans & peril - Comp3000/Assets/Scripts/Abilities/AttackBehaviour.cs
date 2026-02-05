using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Attack")]
public class AttackBehaviour : AbilityBehaviour
{
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            float totalDamage = target.CalculateAbilityDamage(user, target, ability);
            if(totalDamage <0)
            {
                totalDamage = 0;
            }
            Debug.Log($"{user.name} calculated damager as {totalDamage}");

            bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True;



            target.TakeDamage(user,Mathf.RoundToInt(totalDamage), physical, true);
            user.ContributionPoints += 1;
        }
       
    }
}


