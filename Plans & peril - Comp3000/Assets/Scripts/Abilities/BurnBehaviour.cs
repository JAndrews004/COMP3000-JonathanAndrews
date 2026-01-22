using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Burn")]
public class BurnBehaviour : AbilityBehaviour
{
    public int duration;
    public int damage;
    public float defenseReduction;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                BurnEffect effectToAdd;
                if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
                {
                    effectToAdd = new BurnEffect(duration + ability.boost.addedDuration, Mathf.RoundToInt(damage * ability.boost.multipliedPotency), defenseReduction * ability.boost.multipliedPotency, user);
                }
                else
                {
                    effectToAdd = new BurnEffect(duration, damage, defenseReduction, user);
                }
                
                effectToAdd.name = ability.abilityName;
                effectToAdd.description = ability.description;
                effectToAdd.icon = ability.EffectIcon;
                effectToAdd.colorType = colorType.Negative;
                user.ContributionPoints += 0.5f;
                target.ApplyEffect(effectToAdd,true);
                
            }

        }
    }
}
