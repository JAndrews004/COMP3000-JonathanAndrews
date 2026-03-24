using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Passive/AddedShield")]
public class AddedShieldPassive : PassiveBehaviour
{
    public float HPPercentageShield;
    public override void Apply(CombatMember member)
    {
        int shieldToAdd = Mathf.RoundToInt(member.CurrentMaxHealth * HPPercentageShield);

        member.AddShield(shieldToAdd);
    }
}
