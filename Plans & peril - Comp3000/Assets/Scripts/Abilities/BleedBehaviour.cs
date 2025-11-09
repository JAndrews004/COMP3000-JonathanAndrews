using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Bleed")]
public class BleedBehaviour : AbilityBehaviour
{
    public int duration;
    public float damageMult;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                target.ApplyEffect(new BleedEffect(duration, damageMult, user));
            }
        }
    }
}