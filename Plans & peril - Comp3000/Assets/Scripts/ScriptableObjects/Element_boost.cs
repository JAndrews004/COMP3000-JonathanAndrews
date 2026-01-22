using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Boost")]
public class Element_boost : ScriptableObject
{
    public int addedDuration;
    public float multipliedPotency;
    public int additionalTargets;
}
