using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Revive")]
public class ReviveBehaviour : AbilityBehaviour
{
    public float percentageOfHpRestored; 
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        user.ContributionPoints += 1.2f;
        foreach (var target in targets)
        {
            target.Revive(percentageOfHpRestored);
            target.SpawnReviveEffect();
        }
        
    }
}
