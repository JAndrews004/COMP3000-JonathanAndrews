using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Bleed")]
public class BleedBehaviour : AbilityBehaviour
{
    public int duration;
    public float damageMult;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                BleedEffect effectToAdd;
                if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
                {
                    effectToAdd = new BleedEffect(duration+ability.boost.addedDuration, damageMult*ability.boost.multipliedPotency, user);
                }
                else
                {
                    effectToAdd = new BleedEffect(duration, damageMult, user);
                }
                
                effectToAdd.name = ability.abilityName;
                effectToAdd.description = ability.description;
                effectToAdd.icon = ability.EffectIcon;
                effectToAdd.colorType = colorType.Negative;
                user.ContributionPoints += 0.5f;
                target.ApplyEffect(effectToAdd, true);
                
            }
        }
    }
}