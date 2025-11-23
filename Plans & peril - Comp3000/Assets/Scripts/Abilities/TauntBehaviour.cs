using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Behaviours/Taunt")]
public class TauntBehaviour : AbilityBehaviour
{
    public int duration;
    public float softCap;
    public float baseChance;

    public override void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability)
    {

        foreach (var target in targets)
        {
            float rawChance = baseChance + (user.CurrentDefense * 0.3f)- (target.CurrentMagicDefense * 0.2f)+ (user.CurrentLuck * 0.1f);
            float tauntChance = softCap - (softCap - rawChance) / (1 + (rawChance / softCap));

            if (Random.Range(0, 100) <= tauntChance)
            {
                foreach (Effect effect in target.activeEffects)
                {
                    if (effect is TauntEffect && effect.User == user)
                    {
                        effect.duration = duration;
                        return;
                    }
                }
                TauntEffect effectToAdd = new TauntEffect(duration, user);
                effectToAdd.name = ability.abilityName;
                effectToAdd.description = ability.description;
                effectToAdd.icon = ability.icon;
                effectToAdd.colorType = colorType.Negative;
                user.ContributionPoints += 1f;
                target.ApplyEffect(effectToAdd,false);
                
            }




        }
    }

}
