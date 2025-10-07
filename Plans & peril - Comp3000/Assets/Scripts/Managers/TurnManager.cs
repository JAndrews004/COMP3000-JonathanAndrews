using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers;
    public List<Turn> turns = new List<Turn>();

    public CombatManager combatManager;

    private PartyMember SelectedCharacter;
    private string SelectedAction;
    private EnemyMember SelectedTarget;

    public event Action OnPlayerPhaseStart;
    public event Action OnEnemyPhaseStart;

    public event Action<PartyMember> OnCharacterSelected;
    public event Action<EnemyMember> OnTargetSelected;
    public event Action OnSelectingCharacter;
    public event Action OnChoosingAction;
    public event Action OnSelectingTarget;
    public event Action<string> OnActionSelected;
    public event Action<PartyMember, EnemyMember> OnConfirmRequired;
    public event Action<PartyMember, string, EnemyMember> OnActionResolved;

    public void StartCombat()
    {
        Debug.Log("Starting Combat from TM");
        combatManager = GetComponentInParent<CombatManager>();
        PartyMembers = GameManager.Instance.PartyMembers;
        EnemyMembers = GameManager.Instance.EnemyMembers;

        StartPlayerPhase();
    }

    public void SetSelectedCharacter(PartyMember member)
    {
        SelectedCharacter = member;
        OnCharacterSelected?.Invoke(member);
        OnChoosingAction?.Invoke();
    }

    public void SetChosenAction(string action)
    {
        SelectedAction = action;
        OnActionSelected?.Invoke(action);

        // Tell the UI / CombatManager to show target buttons
        OnSelectingTarget?.Invoke();
    }


    public void PlayerSelectedTarget(EnemyMember target)
    {
        SelectedTarget = target;
        OnTargetSelected?.Invoke(target);
        OnConfirmRequired?.Invoke(SelectedCharacter, SelectedTarget);

        // Update the UI state of the currently active PartyMemberView
        var view = combatManager?.currentActiveView?.GetComponent<PartyMemberView>();
        view?.viewModel?.RequireConfirm();
    }



    public void ConfirmAction()
    {
        if (SelectedCharacter != null && SelectedCharacter.HasTurn &&
            SelectedAction != null && SelectedTarget != null)
        {
            turns.Add(new Turn(SelectedTarget, SelectedAction, SelectedCharacter));
            SelectedCharacter.HasTurn = false;
            OnActionResolved?.Invoke(SelectedCharacter, SelectedAction, SelectedTarget);

            var vm = combatManager.FindPartyMemberViewModel(SelectedCharacter);
            vm?.DisableSelection();
        }

        if (PartyMembers.All(m => !m.HasTurn))
            ExecutePlayerActions();
    }

    public void ExecutePlayerActions()
    {
        foreach (var t in turns)
        {
            PlayerSelectedAttack(t.Attacker);
        }
        EndPlayerPhase();
    }

    public void PlayerSelectedAttack(PartyMember model)
    {
        Debug.Log($"{model.baseStats.characterName} attacked!");
        EnemyMembers[0].TakeDamage(model.CurrentAttack);
    }

    public void StartPlayerPhase()
    {
        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = null;
        turns.Clear();

        foreach (var member in PartyMembers) member.HasTurn = true;

        StartCoroutine(DelayedPlayerPhaseStart());
    }

    private IEnumerator DelayedPlayerPhaseStart()
    {
        yield return null; // wait one frame
        OnPlayerPhaseStart?.Invoke();
        OnSelectingCharacter?.Invoke();
    }

    public void EndPlayerPhase()
    {
        
        CheckWinLoss();
        StartEnemyPhase();
    }

    private void StartEnemyPhase()
    {
        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = null;
        turns.Clear();

        OnEnemyPhaseStart?.Invoke();
    }

    public void ExecuteEnemyActions()
    {
        foreach (var enemy in EnemyMembers.Where(e => e.Alive))
        {
            var target = PartyMembers.FirstOrDefault(p => p.Alive);
            if (target != null) enemy.BasicAttack(target);

            Debug.Log($"{enemy.baseStats.characterName} attacked {target.baseStats.characterName} for {enemy.CurrentAttack} damage");
        }
        EndEnemyPhase();
    }

    private void EndEnemyPhase()
    {
        CheckWinLoss();
        StartPlayerPhase();
    }

    private void CheckWinLoss()
    {
        if (EnemyMembers.All(e => !e.Alive))
        {
            Debug.Log("All enemies defeated! You win!");
            combatManager.EndCombat();
            GameManager.Instance.EndCombat();
        }
        else if (PartyMembers.All(p => !p.Alive))
        {
            Debug.Log("All players defeated! Game over!");
            combatManager.EndCombat();
            GameManager.Instance.EndCombat();
        }
    }

    public void StartCharacterSelection()
    {
        OnSelectingCharacter?.Invoke();
    }

    public void InvokeOnSelectingTarget()
    {
        OnSelectingTarget?.Invoke();
    }

}
