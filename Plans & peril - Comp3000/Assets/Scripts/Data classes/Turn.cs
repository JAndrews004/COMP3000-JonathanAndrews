using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public CombatMember Attacker;
    public List<CombatMember> Target { get; set; }
    public Ability Action { get; set; }

    public Turn(List<CombatMember> target, Ability action, CombatMember attacker)
    {
        Target = new List<CombatMember>(target);
        Action = action;
        Attacker = attacker;
    }
}
