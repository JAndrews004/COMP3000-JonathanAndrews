using UnityEngine;

public abstract class AbilityBehaviour : ScriptableObject
{
    
    public abstract void Execute(CombatMember user, CombatMember[] targets);
}
