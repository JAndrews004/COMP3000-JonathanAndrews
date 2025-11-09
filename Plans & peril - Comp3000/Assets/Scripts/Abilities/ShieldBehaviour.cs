using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Shield")]
public class ShieldBehaviour : AbilityBehaviour
{
    public int ShieldValue;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            target.AddShield(ShieldValue);
        }
    }
}
