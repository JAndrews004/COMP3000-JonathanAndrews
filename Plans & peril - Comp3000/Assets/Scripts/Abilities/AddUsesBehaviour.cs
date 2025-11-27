using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/AddUses")]
public class AddUsesBehaviour : AbilityBehaviour
{
    public int usesAdded;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            foreach (Ability ab in target.activeAbilities)
            {
                ab.usesLeft += usesAdded;
                if (ab.usesLeft > ab.AbilityData.maxUsage)
                {
                    ab.usesLeft = ab.AbilityData.maxUsage;
                }
            }

            user.ContributionPoints += 0.75f;
        }
        
    }
}