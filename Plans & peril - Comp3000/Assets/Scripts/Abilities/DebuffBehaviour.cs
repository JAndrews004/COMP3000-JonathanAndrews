using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Debuff")]
public class DebuffBehaviour : AbilityBehaviour
{
    public List<StatType> statsToDebuff;
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
                if(Random.Range(0,100) <= target.GetEffectApplyChance(user, ability))
                {
                    foreach (StatType statToDebuff in statsToDebuff)
                    {
                        DebuffEffect effectToAdd;
                        if (target.element == Element.Earth)
                        {
                            effectToAdd = new DebuffEffect(statToDebuff, maxPercent, durationTurns-1);
                        }
                        else
                        {
                            effectToAdd = new DebuffEffect(statToDebuff, maxPercent, durationTurns);
                        }
                        effectToAdd.name = ability.abilityName;
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.EffectIcon;
                        effectToAdd.colorType = colorType.Negative;

                        user.ContributionPoints += 0.5f;
                        target.ApplyEffect(effectToAdd, true);
                    }
                    target.SpawnDebuffEffect();


                }
                
            }
            else
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    foreach (StatType statToDebuff in statsToDebuff)
                    {
                        DebuffEffect effectToAdd = new DebuffEffect(statToDebuff, scaledPercentage, durationTurns);
                        effectToAdd.name = ability.abilityName;
                        effectToAdd.description = ability.description;
                        effectToAdd.icon = ability.EffectIcon;
                        effectToAdd.colorType = colorType.Negative;
                        user.ContributionPoints += 0.5f;
                        target.ApplyEffect(effectToAdd, true);
                    }
                    target.SpawnDebuffEffect();

                }

            }

            
        }
    }
}
