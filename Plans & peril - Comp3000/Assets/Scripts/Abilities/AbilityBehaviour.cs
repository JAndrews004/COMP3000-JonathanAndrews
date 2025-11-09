using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class AbilityBehaviour : ScriptableObject
{
    public int baseDamage;
    public abstract void Execute(CombatMember user, List<CombatMember> targets, AbilityData ability);
}
