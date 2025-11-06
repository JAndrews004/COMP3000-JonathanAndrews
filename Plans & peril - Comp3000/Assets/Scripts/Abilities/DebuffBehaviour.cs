using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Debuff")]
public class DebuffBehaviour : AbilityBehaviour
{
    public StatType statToDebuff;
    public float percentageDecrease;
    public int durationTurns;

    public override void Execute(CombatMember user, List<CombatMember> targets)
    {
        foreach (var target in targets)
        {
            target.ApplyEffect(new DebuffEffect(statToDebuff, percentageDecrease, durationTurns));
        }
    }
}
