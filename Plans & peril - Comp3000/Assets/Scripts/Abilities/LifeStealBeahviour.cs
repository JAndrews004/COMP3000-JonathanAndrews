using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/LifeSteal")]
public class LifeStealBeahviour : AbilityBehaviour
{
    public float percentageStolen;
    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {
        foreach (var target in targets)
        {
            float totalDamage = target.CalculateAbilityDamage(user, target, ability);
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            Debug.Log($"{user.name} calculated damage as {totalDamage}");

            bool physical = ability.powerType == AbilityPowerType.Physical
                || ability.powerType == AbilityPowerType.True;

            int healing = Mathf.RoundToInt(totalDamage * percentageStolen);

            target.TakeDamage(user, Mathf.RoundToInt(totalDamage), physical, true,false);

            target.TakeDamage(user, Mathf.RoundToInt(healing), physical, false, false);
            user.Heal(healing);
            user.ContributionPoints += 1;
        }
        
    }
}