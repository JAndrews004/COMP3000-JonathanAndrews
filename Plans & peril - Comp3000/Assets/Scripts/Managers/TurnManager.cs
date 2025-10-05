using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static event Action<PartyMember> OnPlayerTurnStart;
    public static event Action<PartyMember> OnPlayerTurnEnd;
    public static event Action<EnemyMember> OnEnemyTurnStart;
    public static event Action<EnemyMember> OnEnemyTurnEnd;

    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers;

    private PartyMember currentPlayer;
    private EnemyMember currentEnemy;

    private bool IsPlayerPhase;
    private int CurrentUnitIndex;
    // Start is called before the first frame update
    public void StartCombat()
    {
        PartyMembers = GameManager.Instance.PartyMembers;
        EnemyMembers = GameManager.Instance.EnemyMembers;
        IsPlayerPhase = true;
        CurrentUnitIndex = 0;
        StartNextUnitTurn();
    }

    private void StartNextUnitTurn()
    {
        if (IsPlayerPhase)
        {
            
            if (CurrentUnitIndex >= PartyMembers.Count)
            {
                // Player phase finished ? switch to enemy phase
                IsPlayerPhase = false;
                CurrentUnitIndex = 0;
                StartNextUnitTurn();
                return;
            }

            currentPlayer = PartyMembers[CurrentUnitIndex];
            
            gameObject.GetComponentInParent<CombatManager>().ShowActiveUnitUI(currentPlayer);
            OnPlayerTurnStart?.Invoke(currentPlayer);
            // Enable Attack/End Turn buttons here for Sprint 1
        }
        else
        {
            gameObject.GetComponentInParent<CombatManager>().HideActiveUnitUI();
            if (CurrentUnitIndex >= EnemyMembers.Count)
            {
                // Enemy phase finished ? switch back to player phase
                IsPlayerPhase = true;
                CurrentUnitIndex = 0;
                StartNextUnitTurn();
                return;
            }

            currentEnemy = EnemyMembers[CurrentUnitIndex];
            OnEnemyTurnStart?.Invoke(currentEnemy);
            ExecuteEnemyTurn(currentEnemy);
        }
    }

    public void EndUnitTurn()
    {
        if (IsPlayerPhase)
        {
            currentPlayer = PartyMembers[CurrentUnitIndex];
            OnPlayerTurnEnd?.Invoke(currentPlayer);
        }
        else
        {
            currentEnemy = EnemyMembers[CurrentUnitIndex];
            OnEnemyTurnEnd?.Invoke(currentEnemy);
        }

        CurrentUnitIndex++;
        CheckWinLoss(); // optional, can also do after each action
        StartNextUnitTurn();
    }
    private void ExecuteEnemyTurn(EnemyMember enemy)
    {
        PartyMember target = PartyMembers.FirstOrDefault(p => p.CurrentHealth > 0);
        if (target != null)
        {
            enemy.BasicAttack(target);
        }

       
        EndUnitTurn();
    }
    private void CheckWinLoss()
    {
        bool allEnemiesDead = EnemyMembers.All(enemy => !enemy.Alive);
        bool allPlayersDead = PartyMembers.All(member => !member.Alive);

        if (allEnemiesDead)
        {
            Debug.Log("All enemies defeated! You win!");
                Win();
        }
        else if (allPlayersDead)
        {
            Debug.Log("All party members defeated! Game over!");
                Loss();
        }
    }

    public void PlayerSelectedAttack(PartyMember model)
    {
        Debug.Log("Attacked!");
        EnemyMembers[0].TakeDamage(model.CurrentAttack);
        EndUnitTurn();
    }

    private void Win()
    {
        GameManager.Instance.EndCombat();
    }
    private void Loss()
    {
        GameManager.Instance.EndCombat();
    }
}
