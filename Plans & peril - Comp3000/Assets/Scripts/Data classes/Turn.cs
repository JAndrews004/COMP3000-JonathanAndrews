using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public PartyMember Attacker;
    public EnemyMember Target { get; set; }
    public string Action { get; set; }

    public Turn(EnemyMember target, string action, PartyMember attacker)
    {
        Target = target;
        Action = action;
        Attacker = attacker;
    }
}
