using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public string abilityID;
    public string abilityName;
    [TextArea] public string description;
    public Sprite icon;
    public AbilityCategory abilityCategory;
    public AbilityPowerType powerType;
    public AbilityType abilityType;
    public List<AbilityTag> tags;
    public int maxUsage;
    public int cooldown;
    public TargetType targetType;
    public int numberOfTargets;
    public AbilityBehaviour PhysicalBehaviour;
    public AbilityBehaviour EffectBehaviour;
    public PassiveBehaviour passiveBehaviour;
    public bool guaranteedEffectHit;
    public float EffectChanceScaling;
    public Sprite EffectIcon;
    public bool IsTauntable;

    [Header("Element")]
    public bool isElement;
    public Element elementTag;
    public Element_boost boost;

    [Header("Ability tree data")]
    public bool unlocked;
    public List<AbilityData> prerequisiteAbilities;
    public int strengthRequired;
    public int defenseRequired;
    public int intelligenceRequired;
    public int magicDefenseRequired;
    public int luckRequired;
    public int vitalityRequired;
    public int goldCost;
    public int treeCol;
    public int treeRow;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(abilityID))
        {
            abilityID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
public enum AbilityPowerType
{
    Physical,
    Magical,
    Mixed,
    True // ignores defenses, e.g., poison or pure effects
}
public enum AbilityTag
{
    Damage,
    Heal,
    Buff,
    Debuff,
    Stun,
    Unstun,
    Vulnerability,
    Cleanse,
    Bleed,
    Curse,
    Dispel,
    Guard,
    Immunity,
    LifeSteal,
    Poison,
    Reflect,
    Regen,
    Revive,
    Shield,
    Sleep,
    Taunt,
    Blowback,

}