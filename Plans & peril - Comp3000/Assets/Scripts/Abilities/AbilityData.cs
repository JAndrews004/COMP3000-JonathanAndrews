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
    [HideInInspector] public int usesLeft;
    public int cooldown;
    [HideInInspector] public int cooldownLeft;
    public TargetType targetType;
    public AbilityBehaviour behaviour;

    public void DecreaseUses()
    {
        usesLeft--;

        if (usesLeft <= 0)
        {
            Debug.Log("Uses depleated for: " + abilityName);
            usesLeft = 0;
        }
    }
    public void DecreaseCooldown()
    {
        cooldownLeft--;
        if (cooldownLeft <= 0)
        { 
            cooldownLeft = 0; 
        } 
    }
    private void OnEnable()
    {
        usesLeft = maxUsage; // Reset when reloaded or instantiated
        cooldownLeft = 0;
    }
}
