using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class AbilityBehaviour : ScriptableObject
{
    
    public abstract void Execute(CombatMember user, List<CombatMember> targets);
}
