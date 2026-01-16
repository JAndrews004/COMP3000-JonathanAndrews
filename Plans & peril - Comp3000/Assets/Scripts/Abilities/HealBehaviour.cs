using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Heal")]
public class HealBehaviour : AbilityBehaviour
{
    public float basePercent;
    public float maxPercent;
    public float intelligenceScaling;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        user.ContributionPoints += 0.8f;
        foreach (var target in targets)
        {
            float baseHeal = target.CurrentMaxHealth * basePercent;
            float scaledHeal = target.CurrentMaxHealth * (user.CurrentIntelligence*intelligenceScaling);

            if (baseHeal + scaledHeal >= target.CurrentHealth * maxPercent) { 
                target.Heal(Mathf.RoundToInt(target.CurrentHealth * maxPercent));
            }
            else
            {
                target.Heal(Mathf.RoundToInt(baseHeal + scaledHeal));
            }
            target.SpawnHealEffect();
        }
    }
}
