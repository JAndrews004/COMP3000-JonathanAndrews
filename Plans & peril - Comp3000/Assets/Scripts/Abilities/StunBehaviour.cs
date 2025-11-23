using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Stun")]

public class StunBehaviour : AbilityBehaviour
{
    public int turnsStunnedFor;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                StunEffect effectToAdd = new StunEffect(turnsStunnedFor);
                effectToAdd.name = ability.abilityName;
                effectToAdd.description = ability.description;
                effectToAdd.icon = ability.icon;
                effectToAdd.colorType = colorType.Neutral;
                user.ContributionPoints += 0.5f;
                target.ApplyEffect(effectToAdd, false);
                
            }
        }
    }

}
