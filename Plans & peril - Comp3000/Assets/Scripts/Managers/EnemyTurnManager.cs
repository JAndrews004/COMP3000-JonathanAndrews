using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyTurnManager : MonoBehaviour
{
    public TurnManager tm;
    public List<EnemyMember> enemies;
    bool isEnemyPhase;
    
    public void StartEnemyPhase()
    {
        isEnemyPhase = true;
        StartCoroutine(RunEnemyTurns());
    }

    public IEnumerator RunEnemyTurns()
    {
        foreach (EnemyMember enemy in enemies)
        {
            tm.DebugCombatLog = new debugCombatLog();
            tm.DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };
            tm.DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
            tm.DebugCombatLog.targets = new List<CombatMember> { };
            if (enemy.Alive && !enemy.IsStunned)
            {
                bool canHaveTurn = true;
                foreach(Effect effect in enemy.activeEffects)
                {
                    if(effect.statusEffectType== StatusEffect.Delay)
                    {
                        canHaveTurn = false;
                    }
                }
                if(canHaveTurn)
                yield return enemy.TakeTurn();
            }
            if (tm.CheckWinLoss())
            {
                yield return null;
            }
          
        }
        isEnemyPhase = false;
        tm.EndEnemyPhase();
    }

    public void RegisterEnemies(List<EnemyMember> Enemies)
    {
        enemies = Enemies;
    }
    void ClearEnemies()
    {
        enemies.Clear();
    }
}
