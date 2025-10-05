using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberViewModel
{
    public PartyMember model;

    public event Action<bool> OnTurnStateChanged;
    public event Action<int> OnHealthChanged;

    public TurnManager turnManager;

    private bool canAct;
    public bool CanAct
    {
        get => canAct;
        private set
        {
            if (canAct != value)
            {
                canAct = value;
                OnTurnStateChanged?.Invoke(canAct);
            }
        }
    }
    public PartyMemberViewModel(PartyMember model, TurnManager tm)
    {
        this.model = model;
        this.turnManager = tm;
        Debug.Log($"[ViewModel] Bound to {model.baseStats.characterName} with TurnManager: {tm != null}");
        OnHealthChanged?.Invoke(model.CurrentHealth);
    }

    public void StartTurn()
    {
        CanAct = true;
    }

    public void EndTurn()
    {
        CanAct = false;
        turnManager.EndUnitTurn();
    }

    public void AttackButtonPressed()
    {
        if (!CanAct) return;

        Debug.Log($"{model.baseStats.characterName} attacked!");
        turnManager.PlayerSelectedAttack(model);
    }

    public void EndTurnButtonPressed()
    {
        if (!CanAct) return;

        Debug.Log($"{model.baseStats.characterName} ended their turn!");
        EndTurn();
        
    }

    public void UpdateHealth()
    {
        OnHealthChanged?.Invoke(model.CurrentHealth);
    }
}
