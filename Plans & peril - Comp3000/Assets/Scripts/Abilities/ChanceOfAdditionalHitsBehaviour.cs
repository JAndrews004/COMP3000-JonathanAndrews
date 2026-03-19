using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/ChanceOfAdditionalHitsBehaviour")]
public class ChanceOfAdditionalHitsBehaviour : AbilityBehaviour
{
    public float chanceOfExtraHit;
    public float chanceDecay;
    public int maxHits;
    
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True || ability.powerType == AbilityPowerType.Mixed;
        foreach (var target in targets)
        {
            int hits = 0;
            if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
            {
                hits = CalculateHits(chanceOfExtraHit * ability.boost.multipliedPotency, chanceDecay, maxHits);
            }
            hits = CalculateHits(chanceOfExtraHit, chanceDecay, maxHits);

            for (int i = 0; i < hits; i++)
            {
                if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
                {
                    
                }
                float totalDamage = target.CalculateAbilityDamage(user, target, ability);

                if (totalDamage < 0)
                    totalDamage = 0;

                target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true, false);

                user.ContributionPoints += 1;
            }
        }

    }
    private int CalculateHits(float baseChance, float decay, int maxHits)
    {
        int hits = 1;

        for (int hitIndex = 2; hitIndex <= maxHits; hitIndex++)
        {
            float chance = baseChance * Mathf.Pow(decay, hitIndex - 2);

            if (Random.Range(0f, 1f) <= chance)
            {
                hits++;
            }
            else
            {
                break;
            }
        }

        return hits;
    }
}
