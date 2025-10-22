using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public PartyMember Attacker;
    public CombatMember Target { get; set; }
    public AbilityData Action { get; set; }

    public Turn(CombatMember target, AbilityData action, PartyMember attacker)
    {
        Target = target;
        Action = action;
        Attacker = attacker;
    }
}
