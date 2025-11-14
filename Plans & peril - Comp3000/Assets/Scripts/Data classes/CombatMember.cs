using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;


public abstract class CombatMember : MonoBehaviour
{
    public Characters baseStats;
    public CombatManager combatManager;
    public int CurrentHealth;
    public int CurrentMaxHealth;
    public int CurrentAttack;
    public int CurrentDefense;
    public int CurrentIntelligence;
    public int CurrentMagicDefense;
    public int CurrentLuck;

    public int shieldValue;

    public bool gainImmediateExtraTurn = false;
    public bool gainExtraTurnNextRound = false;
    public bool IsStunned = false;

    public List<AbilityData> abilityDatas;

    public List<Ability> activeAbilities = new List<Ability>() { };
    public List<Ability> passiveAbilities = new List<Ability>();

    public List<Effect> activeEffects = new List<Effect>() { };

    public event Action<CombatMember> OnDeath;
    public event Action<CombatMember, int> OnDamageTaken;
    public event Action<CombatMember> OnHealthChanged;
    public event Action<CombatMember, Ability> OnCastAbility;

    public bool Alive => CurrentHealth > 0;
    


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
        if (activeEffects.Count > 0)
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {


                if (activeEffects[i].duration <= 0)
                {
                    activeEffects[i].Remove(this);
                    activeEffects.RemoveAt(i);
                }
                else
                {
                    activeEffects[i].Tick(this);
                }

            }
        }
    }
    public void TakeDamage(CombatMember attacker,int AttackPower)
    {
        if (activeEffects == null)
        {
            activeEffects = new List<Effect>() { };
        }
        foreach (Effect effect in activeEffects)
        {
            if(effect is SleepEffect)
            {
                effect.Remove(this);
            }
        }

        float dodgeChance = CurrentLuck * 0.2f;

        if(Random.Range(0,100)<= dodgeChance)
        {
            Debug.Log($"{name} dodged the attack");
            return;
        }
        int critChance = attacker.CurrentLuck;
        if (critChance>= 50)
        {
            critChance = 50;
        }
        if(Random.Range(0,100)<= critChance)
        {
            AttackPower = Mathf.RoundToInt(AttackPower * 1.5f);
        }


        if (AttackPower > shieldValue)
        {
            shieldValue = 0;
            AttackPower -= shieldValue;
        }
        else
        {
            shieldValue -= AttackPower;
            return;
        }


        if (CurrentHealth - AttackPower <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke(this);
            OnHealthChanged?.Invoke(this);
            activeEffects.Clear();
        }
        else
        {
            Debug.Log($"{name} is taking {AttackPower} damage");
            CurrentHealth -= AttackPower;
            OnDamageTaken?.Invoke(this, AttackPower);
            OnHealthChanged?.Invoke(this);
        }
    }
    public void Heal(int amount)
    {
        if (Alive)
        {
            CurrentHealth += amount;
            if (CurrentHealth > CurrentMaxHealth)
            {
                CurrentHealth = CurrentMaxHealth;
            }
            OnHealthChanged?.Invoke(this);
        }
        
    }
    public void Revive(float healthRestored)
    {
        CurrentHealth = Mathf.RoundToInt(CurrentMaxHealth * healthRestored);
        OnHealthChanged?.Invoke(this);
    }
    public void ModifyStat(StatType stat, int amount)
    {
        if (stat == StatType.Attack) CurrentAttack += amount;
        else if (stat == StatType.Defense) CurrentDefense += amount;
        else if (stat == StatType.Intelligence) CurrentIntelligence += amount;
        else if (stat == StatType.MaxHealth) CurrentMaxHealth += amount;
        else if (stat == StatType.MagicDefense) CurrentMagicDefense += amount;
        else if (stat == StatType.Luck) CurrentLuck += amount;
    }

    public void ModifyStat(StatType stat, float percentage)
    {
        
        if (stat == StatType.Attack) CurrentAttack += Mathf.RoundToInt(percentage * CurrentAttack);
        else if (stat == StatType.Defense) CurrentDefense += Mathf.RoundToInt(percentage * CurrentDefense);
        else if (stat == StatType.Intelligence) CurrentIntelligence += Mathf.RoundToInt(percentage * CurrentIntelligence);
        else if (stat == StatType.MaxHealth) CurrentMaxHealth += Mathf.RoundToInt(percentage * CurrentMaxHealth);
        else if (stat == StatType.MagicDefense) CurrentMagicDefense += Mathf.RoundToInt(percentage * CurrentMagicDefense);
        else if (stat == StatType.Luck) CurrentLuck += Mathf.RoundToInt(percentage * CurrentLuck);
    }

    public void AddShield(int amount)
    {
        shieldValue += amount;
    }

    public void RemoveShield(int amount)
    {
        shieldValue -= amount;
    }

    public float CalculateAbilityDamage(CombatMember user, CombatMember target, AbilityData ability)
    {
        int baseDamage = ability.PhysicalBehaviour.baseDamage;
        float damage = 0;
        if(ability.powerType == AbilityPowerType.Physical)
        {
            damage = baseDamage * (1 + (user.CurrentAttack / 100)) - target.CurrentDefense;
        }
        else if(ability.powerType == AbilityPowerType.Magical)
        {
            damage = baseDamage * (1 + (user.CurrentIntelligence / 100)) - target.CurrentMagicDefense;
        }
        else if(ability.powerType == AbilityPowerType.True)
        {
            damage = baseDamage;
        }

        return damage;
    }

    public int GetEffectApplyChance(CombatMember user, AbilityData ability)
    {
        if (ability.guaranteedEffectHit)
        {
            return 100;
        }
        else
        {
            return Mathf.RoundToInt(user.CurrentIntelligence * ability.EffectChanceScaling);
        }
       
    }

    public void InitializePassives()
    {
        if (passiveAbilities == null || passiveAbilities.Count == 0)
            return;

        foreach (Ability passive in passiveAbilities)
        {
            if (passive.AbilityData.passiveBehaviour != null)
            {
                passive.AbilityData.passiveBehaviour.Apply(this);
                Debug.Log($"Activated passive: {passive.AbilityData.abilityName}");
            }
        }
    }
    public void RemoveAllPassives()
    {
        if (passiveAbilities == null) return;

        foreach (Ability passive in passiveAbilities)
        {
            if (passive.AbilityData.passiveBehaviour != null)
            {
                passive.AbilityData.passiveBehaviour.Remove(this);
            }
        }
    }


}


