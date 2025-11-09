using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberViewModel
{
    public PartyMember model;
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

    public PartyUIState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState != value)
            {
                currentState = value;
                OnUIStateChanged?.Invoke(currentState);
            }
        }
    }

    private PartyUIState currentState;
    public enum PartyUIState { Hidden, ChoosingAction, SelectingTarget, Confirm, Disabled }

    public event Action<bool> OnTurnStateChanged;
    public event Action<int> OnHealthChanged;
    public event Action<PartyUIState> OnUIStateChanged;

    public PartyMemberViewModel(PartyMember model, TurnManager tm)
    {
        this.model = model;
        this.turnManager = tm;
        OnHealthChanged?.Invoke(model.CurrentHealth);
    }

    public void EnableSelection() => CanAct = true;
    public void DisableSelection() => CanAct = false;

    public void AbilityButtonPressed(Ability ability)
    {
        if (!CanAct) return;

        turnManager.SetChosenAction(ability);

        // Notify CombatManager to show target buttons
        CombatManager cm = GameObject.FindObjectOfType<CombatManager>();
        if (cm != null)
            cm.ShowTargetButtons(); // or SelectingTarget
    }
    

    public void EndTurnButtonPressed()
    {
        if (!CanAct) return;

        //Debug.Log($"{model.baseStats.characterName} ended their turn!");

        // Disable all selections
        var cms = GameObject.FindObjectOfType<CombatManager>();
        cms.DisableAllCharacterSelections();

        //Debug.Log("Diabled selection Buttons");
        // Execute all chosen actions and end the phase
        foreach(PartyMember member in turnManager.PartyMembers)
        {
            member.HasTurn = false;
        }

        turnManager.ConfirmAction();
    }

    public void clearButtonPressed()
    {
        turnManager.SelectedAction = null;
        turnManager.SelectedTarget = new List<CombatMember>();
        turnManager.InvokeSelectingAction();
        var cms = GameObject.FindObjectOfType<CombatManager>();
        cms.DisableAllTargetButtons();
        HideConfirmButton();
    }
    public void OnConfirmButtonPressed()
    {
        if (!CanAct) return;

        //Debug.Log($"{model.baseStats.characterName} confirmed attacked!");
        CombatManager cm = GameObject.FindObjectOfType<CombatManager>();
        if (cm != null)
            cm.OnConfirmButtonPressed(); // or SelectingTarget

    }

    public void HideConfirmButton()
    {
        StartSelectingTarget();
    }


    public void UpdateHealth() => OnHealthChanged?.Invoke(model.CurrentHealth);

    public void StartChoosingAction() => CurrentState = PartyUIState.ChoosingAction;
    public void StartSelectingTarget() => CurrentState = PartyUIState.SelectingTarget;
    public void RequireConfirm() => CurrentState = PartyUIState.Confirm;
    public void DisableUI() => CurrentState = PartyUIState.Disabled;
}
