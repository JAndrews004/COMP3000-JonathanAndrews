using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

[CreateAssetMenu(menuName = "Abilities/Passive/FlatStatBoost")]
public class FlatStatBoost : PassiveBehaviour
{
    public StatType stat;
    public float percentageIncrease;

    public override void Apply(CombatMember member)
    {
        member.ModifyStat(stat, percentageIncrease);
    }
    public override void Remove(CombatMember member)
    {
        
        member.ModifyStat(stat, (1 / (1 + percentageIncrease)) - 1);

    }
}
