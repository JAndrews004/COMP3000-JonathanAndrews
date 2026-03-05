using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

[CreateAssetMenu(menuName = "Abilities/Passive/DamageTypeResistance")]
public class DamageTypeResistance : PassiveBehaviour
{
    public AbilityPowerType resistantPowerType;
    public float damageReduction;
    public override void Apply(CombatMember member)
    {
        ResistanceEffect effect = new ResistanceEffect(resistantPowerType, damageReduction);
        member.ApplyEffect(effect,false);
    }

}
