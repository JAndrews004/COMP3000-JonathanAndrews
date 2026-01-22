using System;
using System.Collections.Generic;
using System.Linq;
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
    [HideInInspector]
    public CombatMember PlayerKilledBy;
    [HideInInspector]
    public float ContributionPoints = 0;

    public Element element;
    public abstract void SpawnBuffEffect();
    public abstract void SpawnDebuffEffect();
    public abstract void SpawnHealEffect();
    public abstract void SpawnReviveEffect();
    public abstract void SpawnStunEffect();
    
    public void ApplyEffect(Effect effect,bool reflectable)
    {
        if(activeEffects == null)
        {
            Debug.LogWarning($"{name}: activeEffects was null - initializing manually");
            activeEffects = new List<Effect>();
        }
        foreach (ImmunityEffect Effect in activeEffects.OfType<ImmunityEffect>())
        {
            if(effect is DebuffEffect or TauntEffect or SleepEffect or InterferenceEffect)
            {
                return;
            }
        }

        if (reflectable)
        {
            foreach (ReflectEffect Effect in activeEffects.OfType<ReflectEffect>())
            {
                if (Effect.refelctEffects)
                {
                    float reflectChance = Effect.chanceOfEffectReflect + this.CurrentLuck * 0.002f;

                    reflectChance = Mathf.RoundToInt(reflectChance * 100);

                    if (Random.Range(0, 100) <= reflectChance)
                    {
                        effect.User.ApplyEffect(effect, false);
                        return;
                    }
                }
            }
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
    public void TakeDamage(CombatMember attacker,int AttackPower,bool physical,bool reflectable)
    {
        if (activeEffects == null)
        {
            activeEffects = new List<Effect>() { };
        }
        foreach (SleepEffect effect in activeEffects.OfType<SleepEffect>())
        {
            effect.Remove(this);
        }
        float damagePercentage = 1.0f;
        int maxDodgeChance = 60;
        double Kd = 50.0;
        double dodgeChance = maxDodgeChance * (1 - Math.Exp(-((double)CurrentLuck / Kd)));

        if (Random.Range(0,100)<= dodgeChance)
        {
            damagePercentage *= 0.5f;
            Debug.Log($"{name} dodged the attack");
        }
        int maxCritChance = 50;
        double Kc = 35.0;
        double critChance = maxCritChance * (1- Math.Exp(-((double)attacker.CurrentLuck / Kc)));
        if (critChance>= 50)
        {
            critChance = 50;
        }
        if(Random.Range(0,100)<= critChance)
        {
            damagePercentage *= 1.5f;
        }

        AttackPower = Mathf.RoundToInt(AttackPower * damagePercentage);
        if (attacker.element == Element.Fire)
        {
            AttackPower = Mathf.RoundToInt(AttackPower * 1.15f);
        }

        switch (attacker.element)
        {
            case Element.Fire:
                if (element == Element.Air)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 1.2f);
                }
                else if( element == Element.Water)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 0.8f);
                }
                break;
            case Element.Air:
                if (element == Element.Earth)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 1.2f);
                }
                else if (element == Element.Fire)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 0.8f);
                }
                break;
            case Element.Earth:
                if (element == Element.Water)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 1.2f);
                }
                else if (element == Element.Air)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 0.8f);
                }
                break;
            case Element.Water:
                if (element == Element.Fire)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 1.2f);
                }
                else if (element == Element.Earth)
                {
                    AttackPower = Mathf.RoundToInt(AttackPower * 0.8f);
                }
                break;
        }
        if (reflectable)
        {
            foreach (ReflectEffect effect in activeEffects.OfType<ReflectEffect>())
            {
                float reflectDamage = 0;
                if (effect is ReflectEffect)
                {

                    if (effect.reflectDamage)
                    {
                        if (physical)
                        {
                            reflectDamage = effect.damageRefelctionPercent + this.CurrentDefense * 0.003f;
                        }
                        else
                        {
                            reflectDamage = effect.damageRefelctionPercent + this.CurrentMagicDefense * 0.003f;
                        }
                    }
                }

                attacker.TakeDamage(this, Mathf.RoundToInt(AttackPower * Mathf.Clamp(reflectDamage, 0, 1)), true,false);
            }
        }
        foreach (VulnerabilityEffect effect in activeEffects.OfType<VulnerabilityEffect>())
        {
            AttackPower =Mathf.RoundToInt( AttackPower*(1 + effect.percentageIncrease));
        }
        foreach (GuardEffect effect in activeEffects.OfType<GuardEffect>())
        {
            int GuardPower = Mathf.RoundToInt(AttackPower * (effect.percentage));
            effect.User.TakeDamage(attacker,GuardPower,true,false);
            AttackPower -= GuardPower;
        }

        

        if (AttackPower > shieldValue)
        {
            AttackPower -= shieldValue;
            shieldValue = 0;
            
        }
        else
        {
            shieldValue -= AttackPower;
            return;
        }

        
        if (CurrentHealth - AttackPower <= 0)
        {
            CurrentHealth = 0;
            PlayerKilledBy = attacker;
            OnDeath?.Invoke(this);
            OnHealthChanged?.Invoke(this);
            activeEffects.Clear();
            //death animation
        }
        else
        {
            Debug.Log($"{name} is taking {AttackPower} damage");
            CurrentHealth -= AttackPower;
            OnDamageTaken?.Invoke(this, AttackPower);
            OnHealthChanged?.Invoke(this);
            if (this is PartyMember pm)
            {
                foreach (PartySlot slot in combatManager.CharacterPositions)
                {
                    if (slot.CurrentPartyMember == pm)
                    {
                        StartCoroutine(GameManager.Instance.fXManager.SpriteFlash(slot));

                        Debug.Log($"{baseStats.characterName} is {Alive}");

                        float a = Alive ? 1f : 0.5f;
                        GameManager.Instance.fXManager.SetAlpha(slot, a);
                        break;
                       
                    }
                }
            }

            else if (this is EnemyMember em)
            {
                foreach (EnemySlot slot in combatManager.EnemyPositions)
                {
                    if (slot.CurrentEnemyMember == em)
                    {
                        StartCoroutine(GameManager.Instance.fXManager.SpriteFlash(slot));
                        float a = Alive ? 1f : 0.5f;
                        GameManager.Instance.fXManager.SetAlpha(slot, a);
                        break;
                    }
                }
            }
            
        }
    }
    
    public void Heal(int amount)
    {
        if (Alive)
        {
            if(element == Element.Water)
            {
                amount = Mathf.RoundToInt(amount*1.15f);
            }
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
        if (this is PartyMember pm)
        {
            foreach (PartySlot slot in combatManager.CharacterPositions)
            {
                if (slot.CurrentPartyMember == pm)
                {
                    StartCoroutine(GameManager.Instance.fXManager.ShieldFlashEffect(slot));
                    break;

                }
            }
        }

        else if (this is EnemyMember em)
        {
            foreach (EnemySlot slot in combatManager.EnemyPositions)
            {
                if (slot.CurrentEnemyMember == em)
                {
                    StartCoroutine(GameManager.Instance.fXManager.ShieldFlashEffect(slot));
                    break;
                }
            }
        }
    }

    public void RemoveShield(int amount)
    {
        shieldValue -= amount;
    }

    public float CalculateAbilityDamage(CombatMember user, CombatMember target, AbilityData ability)
    {
        float maxDR = 0.70f;
        int kd = 80;
        int baseDamage = ability.PhysicalBehaviour.baseDamage;
        float damage = 0;
        if(ability.powerType == AbilityPowerType.Physical)
        {
            if(target.element == Element.Earth)
            {
                damage = baseDamage * (1 + (user.CurrentAttack / 100)) * (1 - maxDR * (1 - Mathf.Exp(-(target.CurrentDefense*1.15f / kd))));
            }
            else
            {
                damage = baseDamage * (1 + (user.CurrentAttack / 100)) * (1 - maxDR * (1 - Mathf.Exp(-(target.CurrentDefense / kd))));
            }
            
        }
        else if(ability.powerType == AbilityPowerType.Magical)
        {
            if (target.element == Element.Earth)
            {
                damage = baseDamage * (1 + (user.CurrentIntelligence / 100)) * (1 - maxDR * (1 - Mathf.Exp(-(target.CurrentMagicDefense *1.15f / kd))));
            }
            else
            {
                damage = baseDamage * (1 + (user.CurrentIntelligence / 100)) * (1 - maxDR * (1 - Mathf.Exp(-(target.CurrentMagicDefense / kd))));
            }
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


