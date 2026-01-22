using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Behaviours/Guard")]
public class GuardBehaviour : AbilityBehaviour
{
    public int duration;
    public float percentage;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            GuardEffect effectToAdd = new GuardEffect(duration, user, percentage);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Neutral;
            user.ContributionPoints += 0.5f;
            target.ApplyEffect(effectToAdd, false);
        }

    }
}