using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/ExtraTurnImm")]
public class ExtraTurnImmediateBehaviour : AbilityBehaviour
{
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                target.gainImmediateExtraTurn = true;
                user.ContributionPoints += 1f;
            }
        }
    }
}
