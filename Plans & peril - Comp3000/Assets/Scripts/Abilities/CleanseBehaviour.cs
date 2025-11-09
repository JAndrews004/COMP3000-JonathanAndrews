using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Cleanse")]
public class CleanseBehaviour : AbilityBehaviour
{
    
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        List<Effect> effectsToRemove = new List<Effect>();
        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                foreach (Effect effect in target.activeEffects)
                {
                    if (effect is DebuffEffect)
                    {
                        effectsToRemove.Add(effect);
                    }
                }
                foreach (Effect effect in effectsToRemove)
                {
                    effect.Remove(target);
                    target.activeEffects.Remove(effect);
                }
                effectsToRemove.Clear();
            }
        }
    }
}
