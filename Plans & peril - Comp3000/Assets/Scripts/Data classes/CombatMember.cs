using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class CombatMember : MonoBehaviour
{
    
    public int CurrentHealth;
    public int CurrentMaxHealth;
    public int CurrentAttack;
    public int CurrentDefense;
    public int CurrentIntelligence;

    public bool gainImmediateExtraTurn = false;
    public bool gainExtraTurnNextRound = false;
    public bool IsStunned = false;

    public List<AbilityData> abilityDatas;

    public List<Ability> abilities = new List<Ability>();
    public List<Effect> activeEffects = new List<Effect>();


    protected virtual void Awake()
    {
        if (abilities == null)
            abilities = new List<Ability>();

        if (abilityDatas == null)
            abilityDatas = new List<AbilityData>();

        if (activeEffects == null)
            activeEffects = new List<Effect>();
    }


    public void ApplyEffect(Effect effect)
    {
        if(activeEffects == null)
        {
            Debug.LogWarning($"{name}: activeEffects was null — initializing manually");
            activeEffects = new List<Effect>();
        }
        effect.Apply(this);
        activeEffects.Add(effect);
        //Debug.Log("Effect applied");
    }

    public void TickEffects()
    {
        if (activeEffects == null)
        {
            Debug.LogWarning($"{name}: activeEffects was null — initializing manually");
            activeEffects = new List<Effect>();
        }
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick(this);
            if (activeEffects[i].duration <= 0)
            {
                activeEffects[i].Remove(this);
                activeEffects.RemoveAt(i);
            }
        }
    }
    public void TakeDamage(int AttackPower)
    {
        if (CurrentHealth - AttackPower <= 0)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= AttackPower;
        }
    }
    public void Heal(int amount)
    {

        CurrentHealth += amount;
        if (CurrentHealth > CurrentMaxHealth)
        {
            CurrentHealth = CurrentMaxHealth;
        }
    }
    public void ModifyStat(StatType stat, int amount)
    {
        if (stat == StatType.Attack) CurrentAttack += amount;
        else if (stat == StatType.Defense) CurrentDefense += amount;
        else if (stat == StatType.Intelligence) CurrentIntelligence += amount;
        else if (stat == StatType.MaxHealth) CurrentMaxHealth += amount;
    }

    public void AddShield(int amount)
    {
        // Add temporary shield value to absorb damage
    }

    public void RemoveShield(int amount)
    {
        // Remove shield value when expired
    }
}
public abstract class Effect
{
    public int duration; // in turns
    public abstract void Apply(CombatMember target);
    public abstract void Remove(CombatMember target);
    public virtual void Tick(CombatMember target) { duration--; }
}
public enum StatType
{
    Attack,
    Defense,
    Intelligence,
    MaxHealth,
}
public class BuffEffect : Effect
{
    private StatType stat;
    private float percentage; // 0.10 = +10%
    private int flatAmount;   // Optional for flat buffs
    private int appliedAmount; // The actual number added to reset later

    // Constructor for percentage-based buffs
    public BuffEffect(StatType stat, float percentage, int duration)
    {
        this.stat = stat;
        this.percentage = percentage;
        this.duration = duration;
    }

    // Constructor for flat buffs
    public BuffEffect(StatType stat, int flatAmount, int duration)
    {
        this.stat = stat;
        this.flatAmount = flatAmount;
        this.duration = duration;
    }

    public override void Apply(CombatMember target)
    {
        int baseValue = GetStatValue(target, stat);
        appliedAmount = flatAmount > 0
            ? flatAmount
            : Mathf.RoundToInt(baseValue * percentage);

        target.ModifyStat(stat, appliedAmount);
    }

    public override void Remove(CombatMember target)
    {
        target.ModifyStat(stat, -appliedAmount);
    }

    private int GetStatValue(CombatMember target, StatType stat)
    {
        return stat switch
        {
            StatType.Attack => target.CurrentAttack,
            StatType.Defense => target.CurrentDefense,
            StatType.Intelligence => target.CurrentIntelligence,
            StatType.MaxHealth => target.CurrentMaxHealth,
            _ => 0
        };
    }
}

public class DebuffEffect : Effect
{
    private StatType stat;
    private float percentage;
    private int flatAmount;
    private int appliedAmount;

    public DebuffEffect(StatType stat, float percentage, int duration)
    {
        this.stat = stat;
        this.percentage = percentage;
        this.duration = duration;
    }

    public DebuffEffect(StatType stat, int flatAmount, int duration)
    {
        this.stat = stat;
        this.flatAmount = flatAmount;
        this.duration = duration;
    }

    public override void Apply(CombatMember target)
    {
        int baseValue = GetStatValue(target, stat);
        appliedAmount = flatAmount > 0
            ? flatAmount
            : Mathf.RoundToInt(baseValue * percentage);

        target.ModifyStat(stat, -appliedAmount);
    }

    public override void Remove(CombatMember target)
    {
        target.ModifyStat(stat, appliedAmount);
    }

    private int GetStatValue(CombatMember target, StatType stat)
    {
        return stat switch
        {
            StatType.Attack => target.CurrentAttack,
            StatType.Defense => target.CurrentDefense,
            StatType.Intelligence => target.CurrentIntelligence,
            StatType.MaxHealth => target.CurrentMaxHealth,
            _ => 0
        };
    }
}

public class ShieldEffect : Effect
{
    private int shieldAmount;

    public ShieldEffect(int shieldAmount, int duration)
    {
        this.shieldAmount = shieldAmount;
        this.duration = duration;
    }

    public override void Apply(CombatMember target)
    {
        target.AddShield(shieldAmount);
    }

    public override void Remove(CombatMember target)
    {
        target.RemoveShield(shieldAmount);
    }
}

public class StunEffect : Effect
{

    public StunEffect(int duration)
    {
        this.duration = duration;
    }

    public override void Apply(CombatMember target)
    {
        target.IsStunned = true;
    }

    public override void Remove(CombatMember target)
    {
        target.IsStunned = false;
    }
}

