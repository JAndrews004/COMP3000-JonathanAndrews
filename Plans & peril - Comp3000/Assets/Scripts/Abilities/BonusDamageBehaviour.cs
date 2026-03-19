using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/BonusDamageBehaviour")]
public class BonusDamageBehaviour : AbilityBehaviour
{
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            float bonusDamage = 0f;
            bonusDamage = (float)user.damageReceivedPrieviousRound / (float)user.CurrentMaxHealth;

            float totalDamage = target.CalculateAbilityDamage(user, target, ability);
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            Debug.Log($"{user.name} calculated damager as {totalDamage}");

            bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True || ability.powerType == AbilityPowerType.Mixed;

            totalDamage *= (bonusDamage + 1);

            target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true, false);
            user.ContributionPoints += 1;
        }

    }
}
