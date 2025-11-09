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
        DeadAlly,
    }
    public enum AbilityType
    {
        Attack,
        Defense,
        Buff,
        Debuff,
    }
    public enum AbilityCategory
    {
        Active,
        Passive
    }


    public string abilityName;
    [TextArea] public string description;
    public Sprite icon;
    public AbilityCategory abilityCategory;
    public AbilityPowerType powerType;
    public AbilityType abilityType;
    public int maxUsage;
    public int cooldown;
    public TargetType targetType;
    public int numberOfTargets;
    public AbilityBehaviour PhysicalBehaviour;
    public AbilityBehaviour EffectBehaviour;
    public PassiveBehaviour passiveBehaviour;
    public bool guaranteedEffectHit;
    public float EffectChanceScaling;



}
public enum AbilityPowerType
{
    Physical,
    Magical,
    True // ignores defenses, e.g., poison or pure effects
}