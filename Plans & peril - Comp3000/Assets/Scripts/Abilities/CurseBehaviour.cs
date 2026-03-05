using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Curse")]
public class CurseBehaviour : AbilityBehaviour
{
    public int durationTurns;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            CurseEffect effectToAdd = new CurseEffect(durationTurns,user);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Negative;
            user.ContributionPoints += 0.5f;
            target.ApplyEffect(effectToAdd, false);

        }

    }
}

