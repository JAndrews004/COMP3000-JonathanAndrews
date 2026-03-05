using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Passive/ConstantRegen")]
public class ConstantRegen : PassiveBehaviour
{
    public float healthAdded;
    public override void Apply(CombatMember member)
    {
        int healthToAdd = Mathf.RoundToInt(healthAdded * member.CurrentMaxHealth);
        RegenEffect effect = new RegenEffect(100000, member, healthToAdd);

        member.ApplyEffect(effect, false);
    }
}