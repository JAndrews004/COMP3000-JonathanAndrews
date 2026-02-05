using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;

public class CombatSystemTest1
{

    // A Test behaves as an ordinary method
    [Test]
    public void CombatSystemTest_1SimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CombatSystemTest_1WithEnumeratorPasses()
    {
        yield return SceneManager.LoadSceneAsync("Test", LoadSceneMode.Single);
        if (GameManager.Instance == null)
        {
            var Stats = ScriptableObject.CreateInstance<GameStats>();
            Stats.Gold = 1000;
            Stats.passLevel = 1;
            var gmGO = new GameObject("GameManager");
            var gm = gmGO.AddComponent<GameManager>();
            gm.Stats = Stats;
            gm.fXManager = new GameObject("FXManager").AddComponent<FXManager>();
            gm.PartyMembers = new List<PartyMember>();
            gm.EnemyMembers = new List<EnemyMember>();

            GameManager.SetInstanceForTesting(gm);
        }
        for (int i = 0; i < 3; i++)
        {
            var partyGO = new GameObject($"PartyMember_{i}");
            partyGO.transform.parent = GameManager.Instance.transform;

            var statsSO = ScriptableObject.CreateInstance<Characters>();
            statsSO.maxHealth = 200;
            statsSO.attack = 15;
            statsSO.defense = 8;
            statsSO.intelligence = 10;
            statsSO.magicDefence = 7;
            statsSO.Luck = 10;
            statsSO.unlockableAbilities = new List<AbilityData> { CreateBasicAttack() };

            var partyMember = partyGO.AddComponent<PartyMember>();

            partyMember.baseStats = statsSO;
            partyMember.UpdateStats();
            partyMember.abilityDatas = new List<AbilityData> { CreateBasicAttack() };
            GameManager.Instance.PartyMembers.Add(partyMember);
        }
        for (int i = 0; i < 5; i++)
        {
            var enemyGO = new GameObject($"EnemyMember_{i}");
            enemyGO.transform.parent = GameManager.Instance.transform;

            var enemySO = ScriptableObject.CreateInstance<Characters>();
            enemySO.characterName = i.ToString();
            enemySO.maxHealth = 150;
            enemySO.attack = 15;
            enemySO.defense = 8;
            enemySO.intelligence = 10;
            enemySO.magicDefence = 7;
            enemySO.Luck = 10;
            enemySO.unlockableAbilities = new List<AbilityData> { CreateBasicAttack() };

            var enemyMember = enemyGO.AddComponent<EnemyMember>();
            
            enemyMember.baseStats = enemySO;
            enemyMember.UpdateStats(1,EnemyMember.Tier.Easy,5);
            enemyMember.aiController = new EasyAI();
            enemyMember.abilityDatas = new List<AbilityData> { CreateBasicAttack() };
            GameManager.Instance.EnemyMembers.Add(enemyMember);
        }
        var cmGO = new GameObject("CombatManager");
        var cm = cmGO.AddComponent<CombatManager>();

        var tmGO = new GameObject("TurnManager");
        tmGO.transform.parent = cmGO.transform;
        var turnManager = tmGO.AddComponent<TurnManager>();

        var enemyTurnManager = cm.GetComponentInChildren<EnemyTurnManager>();
        if (enemyTurnManager == null)
        {
            
            var etmGO = new GameObject("EnemyTurnManager");
           
            enemyTurnManager = etmGO.AddComponent<EnemyTurnManager>();

            etmGO.transform.parent = cm.transform;
        }

        cm.EnemyPositions = new List<EnemySlot>();
        for (int i = 0; i < GameManager.Instance.EnemyMembers.Count; i++)
        {
            var slotGO = new GameObject($"EnemySlot_{i}");
            slotGO.transform.parent = cm.transform;
            var slot = slotGO.AddComponent<EnemySlot>();
            cm.EnemyPositions.Add(slot);
        }
        cm.CharacterPositions = new List<PartySlot>();
        for (int i = 0; i < GameManager.Instance.PartyMembers.Count; i++)
        {
            var slotGO = new GameObject($"PartySlot_{i}");
            slotGO.transform.parent = cm.transform;
            var slot = slotGO.AddComponent<PartySlot>();
            cm.CharacterPositions.Add(slot);
        }
        turnManager.combatManager = cm;
        turnManager.PartyMembers = GameManager.Instance.PartyMembers;
        enemyTurnManager.RegisterEnemies(GameManager.Instance.EnemyMembers);

        cm.CharacterButtons = new List<GameObject>() { };
        cm.CharacterTargetButtons = new List<GameObject>() { };
        cm.EnemyTargetButtons = new List<GameObject>() { };

        yield return null;
        GameManager.Instance.PrepareCombatData();
        var combatManager = GameObject.FindObjectOfType<CombatManager>();
        Assert.IsNotNull(combatManager, "CombatManager not found");
        yield return null;

        
        if (turnManager == null)
        {
            turnManager = tmGO.AddComponent<TurnManager>();

            tmGO.transform.parent = cm.transform;
        }
        

        combatManager.StartCombat();
        yield return null;

        
        Assert.IsNotNull(turnManager, "TurnManager not found");

        enemyTurnManager.tm = turnManager;
        Assert.IsNotNull(enemyTurnManager, "EnemyTurnManager not found");

        // --- ACT 1: simulate player selecting actions ---
        foreach (var member in turnManager.PartyMembers)
        {
            turnManager.SetSelectedCharacter(member);

            var ability = member.activeAbilities.FirstOrDefault();
            turnManager.SetChosenAction(ability);

            var target = turnManager.EnemyMembers.First();
            turnManager.PlayerSelectedTarget(target);


            turnManager.ConfirmAction();
            yield return null;
        }

        // --- ACT 2: simulate enemy phase ---
        turnManager.ExecuteEnemyActions();
        yield return null;

        // --- ASSERT: Turn order updated correctly ---
        foreach (var member in turnManager.PartyMembers)
        {
            Assert.IsFalse(member.HasTurn, $"{member.baseStats.characterName} still has turn after execution");
        }
        foreach (var enemy in turnManager.EnemyMembers)
        {
            Debug.Log($"Enemy {enemy.name} | HP: {enemy.CurrentHealth} | Alive: {enemy.Alive}");
        }

        foreach (var enemy in turnManager.EnemyMembers)
        {
            Assert.IsTrue(enemy.Alive, $"{enemy.baseStats.characterName} should be alive (or apply your damage logic)");
        }

        // --- ASSERT: Win/Loss condition ---
        bool allEnemiesDead = turnManager.EnemyMembers.All(e => !e.Alive);
        bool allPlayersDead = turnManager.PartyMembers.All(p => !p.Alive);

        foreach (var e in turnManager.EnemyMembers) e.CurrentHealth = 0;
        yield return null;
        turnManager.EndPlayerPhase();
        yield return null;
        Assert.IsTrue(combatManager.win, "CombatManager did not detect victory when all enemies dead");
        foreach (var e in turnManager.EnemyMembers) e.CurrentHealth = 100;
        foreach (var p in turnManager.PartyMembers) p.CurrentHealth = 0;
        yield return null;
        turnManager.EndEnemyPhase();
        yield return null;
        Assert.IsFalse(combatManager.win, "CombatManager did not detect defeat when all players dead");
    }
    private AbilityData CreateBasicAttack()
    {
        var ability = ScriptableObject.CreateInstance<AbilityData>();
        ability.abilityName = "Basic Attack";
        ability.description = "A simple physical attack";
        ability.abilityType = AbilityData.AbilityType.Attack;
        ability.targetType = AbilityData.TargetType.SingleEnemy;
        ability.numberOfTargets = 1;
        ability.maxUsage = 99;
        ability.cooldown = 0;

        ability.PhysicalBehaviour = ScriptableObject.CreateInstance<AttackBehaviour>();
        ability.EffectBehaviour = null;

        return ability;
    }

}
