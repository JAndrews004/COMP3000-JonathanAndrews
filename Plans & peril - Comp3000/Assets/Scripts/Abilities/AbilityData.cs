using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AbilityData;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Basic Ability")]
public class AbilityData : ScriptableObject
{
    public enum TargetType
    {
        SingleEnemy,
        MultipleEnemy,
        AllEnemies,
        SingleAlly,
        MultipleAlly,
        AllAllies,
        None,
    }
    public enum AbilityType
    {
        Attack,
        Defense,
        Buff,
        Debuff,
    }

    public string abilityName;
    [TextArea] public string description;
    public Sprite icon;

    public AbilityType abilityType;
    public int maxUsage;
    public int cooldown;
    public TargetType targetType;
    public int numberOfTargets;
    public AbilityBehaviour behaviour;

    
    
    
}
