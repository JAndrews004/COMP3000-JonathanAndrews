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
                    effect.duration = duration;
                    return;
                }
            }
            ReflectEffect effectToAdd = new ReflectEffect(duration, user,reflectDamage, refelctEffects, damageRefelctionPercent,chanceOfEffectReflect);
            effectToAdd.name = ability.abilityName;
            effectToAdd.description = ability.description;
            effectToAdd.icon = ability.icon;
            effectToAdd.colorType = colorType.Negative;
            user.ContributionPoints += 1f;
            target.ApplyEffect(effectToAdd,false);
        }
    }
}
