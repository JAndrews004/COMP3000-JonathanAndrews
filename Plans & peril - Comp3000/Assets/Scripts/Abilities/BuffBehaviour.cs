using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Buff")]
public class BuffBehaviour : AbilityBehaviour
{
    public StatType statToBuff;
    public int durationTurns;

    public float basePercent;
    public float maxPercent;
    public float intelligenceScaling;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            float scaledPercentage = target.CurrentIntelligence * intelligenceScaling + basePercent;

            if (scaledPercentage >=  maxPercent)
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    target.ApplyEffect(new BuffEffect(statToBuff, maxPercent, durationTurns));
                }
            }
            else
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    target.ApplyEffect(new BuffEffect(statToBuff, scaledPercentage, durationTurns));
                }
            }

            
        }
    }
}

