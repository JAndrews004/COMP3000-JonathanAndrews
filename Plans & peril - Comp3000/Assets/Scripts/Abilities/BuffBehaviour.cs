using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Buff")]
public class BuffBehaviour : AbilityBehaviour
{
    public List<StatType> statsToBuff;
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
                    foreach(StatType statToBuff in statsToBuff)
                    {
                        BuffEffect effectToAdd = new BuffEffect(statToBuff, maxPercent, durationTurns);
                        effectToAdd.name = ability.abilityName;
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.icon;
                        effectToAdd.colorType = colorType.Positive;
                        user.ContributionPoints += 0.5f;
                        target.ApplyEffect(effectToAdd, false);

                    }
                    target.SpawnBuffEffect();
                }
            }
            else
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    foreach (StatType statToBuff in statsToBuff)
                    {
                        BuffEffect effectToAdd = new BuffEffect(statToBuff, scaledPercentage, durationTurns);
                        effectToAdd.name = ability.abilityName;
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.icon;
                        effectToAdd.colorType = colorType.Positive;
                        user.ContributionPoints += 0.5f;
                        target.ApplyEffect(effectToAdd, false);
                    }
                    target.SpawnBuffEffect();
                }
            }

            
        }
    }
}

