using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Passive/Rage")]
public class RagePassive : PassiveBehaviour
{
    public StatType stat;
    public float HpPercentageTrigger;
    public float StatPercentageIncrease;

    bool Applied = false;
    public override void Apply(CombatMember member)
    {
        member.OnHealthChanged += CheckDamageBoost;
    }

    private void CheckDamageBoost(CombatMember member)
    {
       if(((float)member.CurrentHealth / (float)member.CurrentMaxHealth) <= HpPercentageTrigger &&!Applied)
        {
            Debug.Log("Stat Boost activated");
            member.ModifyStat(stat, StatPercentageIncrease);
            Applied = true;
        }
        else if (((float)member.CurrentHealth / (float)member.CurrentMaxHealth) > HpPercentageTrigger && Applied)
        {
            
             member.ModifyStat(stat, (1 / (1 + StatPercentageIncrease)) - 1);
             Applied = false;
            
        }
    }

    public override void Remove(CombatMember member)
    {
        if (Applied)
        {
            member.ModifyStat(stat, (1 / (1 + StatPercentageIncrease)) - 1);
            Applied = false;
        }
        member.OnHealthChanged -= CheckDamageBoost;
    }
}
