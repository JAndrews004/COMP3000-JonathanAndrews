using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Abilities/Passive/FlatStatBoost")]
public class FlatStatBoost : PassiveBehaviour
{
    public List<StatType> statIncrease;
    public float percentageIncrease;

    public List<StatType> statDecrease;
    public float percentageDecrease;

    public override void Apply(CombatMember member)
    {
        foreach (StatType statToBuff in statIncrease)
        {
            member.ModifyStat(statToBuff, percentageIncrease);
        }
        foreach (StatType statToBuff in statDecrease)
        {
            member.ModifyStat(statToBuff, -percentageDecrease);
        }
    }
    public override void Remove(CombatMember member)
    {
        foreach (StatType statToBuff in statIncrease)
        {
            member.ModifyStat(statToBuff, (1 / (1 + percentageIncrease)) - 1);

        }
        foreach (StatType statToBuff in statDecrease)
        {
            member.ModifyStat(statToBuff, (1 / (1 + -percentageDecrease)) - 1);

        }

    }
}


