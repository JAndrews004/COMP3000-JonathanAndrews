using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public int duration; // in turns
    public CombatMember User;
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
    MagicDefense,
    Luck,
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
        Debug.Log($"Applying buff {stat} to {target.name}");
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

public class SleepEffect : Effect
{

    public SleepEffect(int duration)
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
public class PoisonEffect : Effect
{
    int damagePerTick;

    public PoisonEffect(int duration,int damagePerTick, CombatMember user)
    {
        this.duration = duration;
        this.damagePerTick = damagePerTick;
        User = user;
    }
    public override void Apply(CombatMember target)
    {
        target.TakeDamage(User,damagePerTick);
    }

    public override void Remove(CombatMember target)
    {
        
    }
    public override void Tick(CombatMember target)
    {
        target.TakeDamage(User, damagePerTick);
        duration--;
    }


}

public class BurnEffect : Effect
{
    int damagePerTick;
    float DefenseReduction;

    public BurnEffect(int duration, int damagePerTick, float defenseReduction, CombatMember user)
    {
        this.duration = duration;
        this.damagePerTick = damagePerTick;
        this.DefenseReduction = defenseReduction;
    }
    public override void Apply(CombatMember target)
    {
        target.TakeDamage(User,damagePerTick);
        target.ApplyEffect(new DebuffEffect(StatType.Defense, DefenseReduction, duration));
    }

    public override void Remove(CombatMember target)
    {

    }

    public override void Tick(CombatMember target)
    {
        target.TakeDamage(User,damagePerTick);
        duration--;
    }
}

public class BleedEffect : Effect
{
    
    float attackMult;


    public BleedEffect(int duration, float attackMult, CombatMember user)
    {
        this.duration = duration;
        this.User = user;
        this.attackMult = attackMult;
        
    }
    public override void Apply(CombatMember target)
    {
        target.TakeDamage(User,Mathf.RoundToInt(this.User.CurrentAttack*attackMult));
        
    }

    public override void Remove(CombatMember target)
    {

    }

    public override void Tick(CombatMember target)
    {
        target.TakeDamage(User,Mathf.RoundToInt(this.User.CurrentAttack * attackMult));
        duration--;
    }
}