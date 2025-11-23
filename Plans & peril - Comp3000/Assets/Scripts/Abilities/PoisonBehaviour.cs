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
                    PoisonEffect effectToAdd = new PoisonEffect(duration, damage, user);
                    effectToAdd.name = ability.abilityName;
                    effectToAdd.description = ability.description;
                    effectToAdd.icon = ability.icon;
                    effectToAdd.colorType = colorType.Negative;
                    user.ContributionPoints += 0.5f;
                    target.ApplyEffect(effectToAdd, true);
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
                    PoisonEffect effectToAdd = new PoisonEffect(duration, damage, user);
                    effectToAdd.name = ability.abilityName;
                    effectToAdd.description = ability.description;
                    effectToAdd.icon = ability.icon;
                    effectToAdd.colorType = colorType.Negative;
                    user.ContributionPoints += 0.5f;
                    target.ApplyEffect(effectToAdd,true);
                }
            }
            
        }
    }
}
