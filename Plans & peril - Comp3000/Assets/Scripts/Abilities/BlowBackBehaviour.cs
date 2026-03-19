using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/BlowBackBehaviour")]
public class BlowBackBehaviour : AbilityBehaviour
{
    public int damageReceived;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True || ability.powerType == AbilityPowerType.Mixed;
        foreach (var target in targets)
        {
            
            float totalDamage = target.CalculateAbilityDamage(user, target, ability);
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            Debug.Log($"{user.name} calculated damager as {totalDamage}");

            target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true, false);

            user.ContributionPoints += 1;
        }
        user.TakeDamage(user,damageReceived, physical,false, false);
    }
}
