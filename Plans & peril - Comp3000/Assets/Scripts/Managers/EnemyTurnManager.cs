using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (enemy.Alive && !enemy.IsStunned)
            {
                
                yield return enemy.TakeTurn();
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
