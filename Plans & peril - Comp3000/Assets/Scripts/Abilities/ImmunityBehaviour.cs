using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Behaviours/Immunity")]
public class ImmunityBehaviour : AbilityBehaviour
{
    public int duration;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            ImmunityEffect effectToAdd = new ImmunityEffect(duration, user);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Neutral;
            user.ContributionPoints += 0.5f;
            target.ApplyEffect(effectToAdd, false);
        }

    }
}