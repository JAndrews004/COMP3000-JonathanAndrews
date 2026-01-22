using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Shield")]
public class ShieldBehaviour : AbilityBehaviour
{
    public int ShieldValue;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            if (user.element == ability.elementTag && ability.elementTag != Element.None && ability.boost != null)
            {
                target.AddShield(Mathf.RoundToInt(ShieldValue*ability.boost.multipliedPotency));
            }
            else
            {
                target.AddShield(ShieldValue);
            }
            
        }
    }
}
