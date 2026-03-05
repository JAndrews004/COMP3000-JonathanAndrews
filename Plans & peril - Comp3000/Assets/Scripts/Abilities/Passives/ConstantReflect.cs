using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Passive/ConstantReflect")]
public class ConstantReflect : PassiveBehaviour
{
    public bool reflectDamage;
    public bool refelctEffects;
    public float damageRefelctionPercent;
    public float chanceOfEffectReflect;

    public override void Apply(CombatMember member)
    {
        ReflectEffect effect = new ReflectEffect(1000, member, reflectDamage, refelctEffects, damageRefelctionPercent, chanceOfEffectReflect, true);

        member.ApplyEffect(effect, false);
    }
}
