using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Poison")]
public class PoisonBehaviour : AbilityBehaviour
{
    
    public int duration;
    public int damage;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        
        foreach (var target in targets)
        {
            if (target.activeEffects == null)
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    target.ApplyEffect(new PoisonEffect(duration, damage, user));
                }
            }
            else
            {
                if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
                {
                    foreach (Effect effect in target.activeEffects)
                    {
                        if (effect is PoisonEffect && effect.User == user)
                        {
                            effect.duration = duration;
                            return;
                        }
                    }
                    target.ApplyEffect(new PoisonEffect(duration, damage, user));
                }
            }
            
        }
    }
}
