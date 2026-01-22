using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Regen")]
public class regenBehaviour : AbilityBehaviour
{
    public int duration;
    public int healthAdded;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            RegenEffect effectToAdd = new RegenEffect(duration, user, healthAdded);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Positive;
            user.ContributionPoints += 0.5f;
            target.ApplyEffect(effectToAdd, false);
            target.SpawnHealEffect();
        }

    }
}
