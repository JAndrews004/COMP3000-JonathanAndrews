using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/DelayedAttack")]
public class DelayedAttack : AbilityBehaviour
{
    public int roundsWaiting;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            DelayEffect effectToAdd = new DelayEffect(roundsWaiting,user,target,ability.PhysicalBehaviour,ability);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Negative;
            user.ContributionPoints += 1.0f;
            target.ApplyEffect(effectToAdd, true);
        }

    }
}