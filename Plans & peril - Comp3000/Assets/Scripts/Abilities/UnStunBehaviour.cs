using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Un-stun")]

public class UnStunBehaviour : AbilityBehaviour
{
    public int damage;
    public int turnsStunnedFor;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            foreach(Effect effect in target.activeEffects)
            {
                if (effect is StunEffect)
                {
                    user.ContributionPoints += 0.5f;
                    effect.Remove(target);
                }
            }
        }
    }

}
