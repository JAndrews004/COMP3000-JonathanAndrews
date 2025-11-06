using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Stun")]

public class StunBehaviour : AbilityBehaviour
{
    public int damage;
    public int turnsStunnedFor;

    public override void Execute(CombatMember user, List<CombatMember> targets)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(damage);
            target.ApplyEffect(new StunEffect(turnsStunnedFor));
        }
    }

}
