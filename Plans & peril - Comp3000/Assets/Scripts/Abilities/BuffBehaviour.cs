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

            if (scaledPercentage >= maxPercent)
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability)) // random chance to apply for certain abilities
                {
                    foreach (StatType statToBuff in statsToBuff) // for multi-stat buffs
                    {
                        BuffEffect effectToAdd;
                        if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null) // checking for elemental synergy
                        {
                            effectToAdd = new BuffEffect(statToBuff, maxPercent * ability.boost.multipliedPotency,
                                durationTurns + ability.boost.addedDuration);
                        }
                        else
                        {
                            effectToAdd = new BuffEffect(statToBuff, maxPercent, durationTurns);
                        }
                        effectToAdd.name = ability.abilityName; // setting data for the tooltip on effect icon
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.EffectIcon;
                        effectToAdd.colorType = colorType.Positive;
                        user.ContributionPoints += 0.5f;
                        target.ApplyEffect(effectToAdd, false);

                    }
                    target.SpawnBuffEffect(); // spawns particle FX on character
                }
            }
            else
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    foreach (StatType statToBuff in statsToBuff)
                    {
                        BuffEffect effectToAdd;
                        if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
                        {
                            effectToAdd = new BuffEffect(statToBuff, scaledPercentage * ability.boost.multipliedPotency, durationTurns + ability.boost.addedDuration);
                        }
                        else
                        {
                            effectToAdd = new BuffEffect(statToBuff, scaledPercentage, durationTurns);
                        }
                        effectToAdd.name = ability.abilityName;
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.EffectIcon;
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

