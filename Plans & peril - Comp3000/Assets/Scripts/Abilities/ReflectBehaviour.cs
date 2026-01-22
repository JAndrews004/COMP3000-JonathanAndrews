using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Behaviours/Reflect")]
public class ReflectBehaviour : AbilityBehaviour
{
    public int duration;
    public bool reflectDamage;
    public bool refelctEffects;
    public float damageRefelctionPercent;
    public float chanceOfEffectReflect;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            foreach (Effect effect in target.activeEffects)
            {
                if (effect is ReflectEffect && effect.User == user)
                {
                    if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
                    {
                        effect.duration = duration+ability.boost.addedDuration;
                    }
                    else
                    {
                        effect.duration = duration;
                    }
                    return;
                }
            }
            ReflectEffect effectToAdd;
            if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
            {
                effectToAdd = new ReflectEffect(duration + ability.boost.addedDuration, user, reflectDamage, refelctEffects, damageRefelctionPercent * ability.boost.multipliedPotency, chanceOfEffectReflect * ability.boost.multipliedPotency);
            }
            else
            {
                effectToAdd = new ReflectEffect(duration, user, reflectDamage, refelctEffects, damageRefelctionPercent, chanceOfEffectReflect);
            }
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.EffectIcon;
            effectToAdd.colorType = colorType.Negative;
            user.ContributionPoints += 1f;
            target.ApplyEffect(effectToAdd,false);
        }
    }
}
