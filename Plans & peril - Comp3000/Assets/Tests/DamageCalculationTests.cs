using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DamageCalculationTests
{
    CombatMember attacker;
    CombatMember target;
    AbilityData ability;

    [SetUp]
    public void Setup()
    {
        var attackerGO = new GameObject();
        attacker = attackerGO.AddComponent<TestCombatMember>();

        var targetGO = new GameObject();
        target = targetGO.AddComponent<TestCombatMember>();

        ability = CreateBasicAttack();

        attacker.CurrentAttack = 0;
        target.CurrentDefense = 0;
    }

    [TestCase(10, 0)]
    [TestCase(50, 50)]
    [TestCase(100, 200)]
    public void CalculateAbilityDamage_ReturnsExpectedValue(int attack, int defense)
    {
        attacker.CurrentAttack = attack;
        target.CurrentDefense = defense;

        float maxDR = 0.7f;
        float kd = 80f;
        float baseDamage = 100f;

        float raw = baseDamage * (1f + (float)attack / 100f);
        float expected = raw * (1f - maxDR * (1f - Mathf.Exp(-((float)defense / kd))));

        float actual = attacker.CalculateAbilityDamage(attacker, target, ability);

        Assert.AreEqual(expected, actual, 0.01f);
    }
    [Test]
    public void HigherDefense_ShouldNeverIncreaseDamage()
    {
        attacker.CurrentAttack = 50;

        target.CurrentDefense = 10;
        float lowDefDamage = attacker.CalculateAbilityDamage(attacker, target, ability);

        target.CurrentDefense = 200;
        float highDefDamage = attacker.CalculateAbilityDamage(attacker, target, ability);

        Assert.Less(highDefDamage, lowDefDamage);
    }


    private AbilityData CreateBasicAttack()
    {
        var ability = ScriptableObject.CreateInstance<AbilityData>();
        ability.abilityName = "Basic Attack";
        ability.description = "A simple physical attack";
        ability.abilityType = AbilityData.AbilityType.Attack;
        ability.powerType = AbilityPowerType.Physical;
        ability.targetType = AbilityData.TargetType.SingleEnemy;
        ability.numberOfTargets = 1;
        ability.maxUsage = 99;
        ability.cooldown = 0;

        ability.PhysicalBehaviour = ScriptableObject.CreateInstance<AttackBehaviour>();
        ability.PhysicalBehaviour.baseDamage = 100;
        ability.EffectBehaviour = null;

        return ability;
    }
    public class TestCombatMember : CombatMember
    {
        public override void SpawnBuffEffect() { }
        public override void SpawnDebuffEffect() { }
        public override void SpawnHealEffect() { }
        public override void SpawnReviveEffect() { }
        public override void SpawnStunEffect() { }
    }

}

