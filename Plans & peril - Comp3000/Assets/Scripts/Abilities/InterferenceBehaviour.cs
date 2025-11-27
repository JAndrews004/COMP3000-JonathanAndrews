using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Interference")]
public class InterferenceBehaviour : AbilityBehaviour
{
    public int duration;
    public int newUses;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            InterferenceEffect effectToAdd = new InterferenceEffect(duration,user, newUses);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.icon;
            effectToAdd.colorType = colorType.Negative;
            user.ContributionPoints += 0.5f;
            target.ApplyEffect(effectToAdd, false);
        }
        
    }
}