using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Abilities/Behaviours/DrainUses")]
public class DrainUsesBehaviour : AbilityBehaviour
{
    public int usesDrained;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            foreach (Ability ab in target.activeAbilities)
            {
                ab.usesLeft -= usesDrained;
                if (ab.usesLeft <= 0)
                {
                    ab.usesLeft = 1;
                }
            }
        }
        user.ContributionPoints += 0.75f;
    }
}