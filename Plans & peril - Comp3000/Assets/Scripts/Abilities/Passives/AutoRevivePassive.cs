using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Passive/AutoRevive")]
public class AutoRevivePassive : PassiveBehaviour
{
    bool hasRevived = false;
    public float revivePercentage;
    public override void Apply(CombatMember owner)
    {
        owner.OnDeath += ReviveOnce;
    }

    private void ReviveOnce(CombatMember member)
    {
        if (!hasRevived)
        {
            member.Revive(revivePercentage);
            Debug.Log("Revive used");
            hasRevived = true;
        }
    }

    public override void Remove(CombatMember owner)
    {
        hasRevived = false;
        owner.OnDeath -= ReviveOnce;
    }
}
