using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Dispel")]
public class DispelBehaviour : AbilityBehaviour
{

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                if (target.activeEffects == null)
                {
                    return;

                }
                foreach (Effect effect in target.activeEffects)
                {
                    if (effect is BuffEffect)
                    {
                        effectsToRemove.Add(effect);
                    }
                }
                foreach (Effect effect in effectsToRemove)
                {
                    effect.Remove(target);
                    target.activeEffects.Remove(effect);
                }
                user.ContributionPoints += 0.8f;
                effectsToRemove.Clear();
            }
        }
    }
}