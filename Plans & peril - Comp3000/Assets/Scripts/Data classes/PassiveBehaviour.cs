using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveBehaviour: ScriptableObject
{
    public abstract void Apply(CombatMember owner);
    public virtual void Remove(CombatMember owner) { }
}
