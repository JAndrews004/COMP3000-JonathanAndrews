using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Passive/ImmunityToEffects")]
public class ImmunityToEffects : PassiveBehaviour
{
    public List<StatusEffect> EffectsResistantTo;
    
    public override void Apply(CombatMember member)
    {

        ImmunityToEffect effect = new ImmunityToEffect(EffectsResistantTo);
        member.ApplyEffect(effect, false);
    }

}