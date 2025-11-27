using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/CooldownReset")]
public class CooldownResetBehaviour : AbilityBehaviour
{
   
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            foreach(Ability ab in target.activeAbilities)
            {
                ab.cooldownLeft = 0;
            } 
            user.ContributionPoints += 1.5f;
        }
       
    }
}