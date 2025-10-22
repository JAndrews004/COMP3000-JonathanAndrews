using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Attack")]
public class AttackBehaviour : AbilityBehaviour
{
    public int baseDamage;

    public override void Execute(CombatMember user, CombatMember[] targets)
    {
        foreach (var target in targets)
        {
            int totalDamage = baseDamage + user.CurrentAttack;
            target.TakeDamage(totalDamage);
        }
    }
}


