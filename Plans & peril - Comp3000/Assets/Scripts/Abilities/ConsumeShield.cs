using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Behaviours/ConsumeShield")]
public class ConsumeShield : AbilityBehaviour
{
    public float multiplier;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            float totalDamage = target.CalculateAbilityDamage(user, target, ability);
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            Debug.Log($"{user.name} calculated damager as {totalDamage}");

            bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True || ability.powerType == AbilityPowerType.Mixed;

           
            if(user.shieldValue > 0)
            {
                float shield = user.shieldValue;
                float maxShield = user.CurrentMaxHealth;

                float dmgMult = 1 + (shield / maxShield);

                user.RemoveShield((int)shield);
                totalDamage *= dmgMult * multiplier;
            }

            target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true, false);
            user.ContributionPoints += 1;
        }

    }
}