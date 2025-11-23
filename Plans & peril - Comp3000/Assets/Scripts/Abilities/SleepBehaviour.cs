using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Sleep")]

public class SleepBehaviour : AbilityBehaviour
{
    public int turnsStunnedFor;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                SleepEffect effectToAdd = new SleepEffect(turnsStunnedFor);
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
