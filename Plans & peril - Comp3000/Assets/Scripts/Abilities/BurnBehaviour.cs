using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Burn")]
public class BurnBehaviour : AbilityBehaviour
{
    public int duration;
    public int damage;
    public float defenseReduction;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            if (Random.Range(0, 100) <= target.GetEffectApplyChance(user, ability))
            {
                target.ApplyEffect(new BurnEffect(duration, damage, defenseReduction, user));
            }

        }
    }
}
