using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Behaviours/AdditionalDamageWithEffect")]
public class AdditionalDamageWithEffect : AbilityBehaviour
{
    public List<StatusEffect> statusEffectTypes;
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

            bool effectPresent = false;
            foreach (StatusEffect statusEffect in statusEffectTypes)
            {
                foreach (Effect effect in target.activeEffects)
                {
                    if (effect.statusEffectType == statusEffect)
                    {
                        effectPresent = true;
                    }
                }
            }

            if (effectPresent)
            {
                totalDamage *= multiplier;
            }

            target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true, false);
            user.ContributionPoints += 1;
        }

    }
}