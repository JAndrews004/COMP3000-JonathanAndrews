using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Buff")]
public class BuffBehaviour : AbilityBehaviour
{
    public StatType statToBuff;
    public float percentageIncrease;
    public int durationTurns;

    public override void Execute(CombatMember user, CombatMember[] targets)
    {
        foreach (var target in targets)
        {
            target.ApplyEffect(new BuffEffect(statToBuff, percentageIncrease, durationTurns));
        }
    }
}

