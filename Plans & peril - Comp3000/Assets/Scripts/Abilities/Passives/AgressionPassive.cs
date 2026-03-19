using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Passive/AgressionPassive")]
public class AgressionPassive : PassiveBehaviour
{
    
    public float multiplier;
    AgressionEffect effect;
    public override void Apply(CombatMember member)
    {
        effect = new AgressionEffect(multiplier);
        member.ApplyEffect(effect,false);
    }
    public override void Remove(CombatMember member)
    {
        member.activeEffects.Remove(effect);

    }
}