using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers = new List<EnemyMember>() { };
    public List<Turn> turns = new List<Turn>();

    public CombatManager combatManager;

    public PartyMember SelectedCharacter;
    public Ability SelectedAction;
    public List<CombatMember> SelectedTarget = new List<CombatMember>();

    public event Action OnPlayerPhaseStart;
    public event Action OnEnemyPhaseStart;

    public event Action<PartyMember> OnCharacterSelected;
    public event Action OnSelectingCharacter;
    public event Action OnChoosingAction;
    public event Action OnSelectingTarget;
    public event Action<Ability> OnActionSelected;
    public event Action<PartyMember, List<CombatMember>> OnConfirmRequired;
    public event Action<PartyMember, Ability, List<CombatMember>> OnActionResolved;

    public void StartCombat()
    {
        //Debug.Log("Starting Combat from TM");
        combatManager = GetComponentInParent<CombatManager>();
        PartyMembers = GameManager.Instance.PartyMembers;
        
        foreach(var Enemyslot in combatManager.EnemyPositions)
        {
            EnemyMembers.Add(Enemyslot.CurrentEnemyMember);
        }
        combatManager.enemyTurnManager.RegisterEnemies(EnemyMembers);
        GameManager.Instance.EnemyMembers = EnemyMembers;
        StartPlayerPhase();
    }

    public void SetSelectedCharacter(PartyMember member)
    {
        SelectedTarget.Clear();

        TurnOffAllCharacterSelecArrows();

        if (SelectedCharacter != null)
        {
            
            FindPartyMemberSlot(member).GetComponent<PartySlot>().TurnCharacterArrowOn();

        }
        
        SelectedCharacter = member;

        

        OnCharacterSelected?.Invoke(member);
        OnChoosingAction?.Invoke();
    }

    public GameObject FindPartyMemberSlot(CombatMember member)
    {
        GameObject selectedSlot = null;

        if (member != null)
        {
            for (int i = 0; i < combatManager.CharacterPositions.Count; i++)
            {
                if (PartyMembers[i] == member)
                {
                    selectedSlot = combatManager.CharacterPositions[i];
                }
            }
            
        }

        return selectedSlot;
    }
    public void SetChosenAction(Ability action)
    {
        SelectedAction = action;
        OnActionSelected?.Invoke(action);

        // Tell the UI / CombatManager to show target buttons
        OnSelectingTarget?.Invoke();
    }

    public EnemySlot FindEnemyMemberSlot(CombatMember member)
    {
        EnemySlot selectedSlot = null;

        if (SelectedTarget != null)
        {
            for (int i = 0; i < combatManager.EnemyPositions.Count; i++)
            {
                if (combatManager.EnemyPositions[i].CurrentEnemyMember == member)
                {
                    selectedSlot = combatManager.EnemyPositions[i];
                }
            }

        }

        return selectedSlot;
    }

    public void PlayerSelectedTarget(CombatMember target)
    {
        if (SelectedTarget == null) { 
            SelectedTarget = new List<CombatMember>();
        }

        if (SelectedTarget.Contains(target))
        {
            SelectedTarget.Remove(target);
            if(target is PartyMember)
            {
                FindPartyMemberSlot(target).GetComponent<PartySlot>().TurnTargetArrowOff();
            }
            else
            {
                FindEnemyMemberSlot(target).GetComponent<EnemySlot>().TurnTargetArrowOff();
            }
        }
        else if (SelectedTarget.Count() < SelectedAction.AbilityData.numberOfTargets)
        {
            SelectedTarget.Add(target);
            if (target is PartyMember)
            {
                FindPartyMemberSlot(target).GetComponent<PartySlot>().TurnTargetArrowOn();
            }
            else
            {
                FindEnemyMemberSlot(target).GetComponent<EnemySlot>().TurnTargetArrowOn();
            }
        }

       
        

        if(SelectedAction.AbilityData.numberOfTargets >= SelectedTarget.Count)
        {
            OnConfirmRequired?.Invoke(SelectedCharacter, SelectedTarget);

            // Update the UI state of the currently active PartyMemberView
            var view = combatManager?.currentActiveView?.GetComponent<PartyMemberView>();
            view?.viewModel?.RequireConfirm();
        }
        else if(SelectedTarget.Count == 0)
        {
            var view = combatManager?.currentActiveView?.GetComponent<PartyMemberView>();
            view?.HideConfirmButton();
        }
        
    }



    public void ConfirmAction()
    {
   
        if (SelectedCharacter != null && SelectedCharacter.HasTurn &&
            SelectedAction != null && SelectedTarget != null && SelectedTarget.Count() >0)
        {
            
            Turn newturn = new Turn(SelectedTarget, SelectedAction, SelectedCharacter);
            turns.Add(newturn);
            SelectedCharacter.HasTurn = false;
           
       
            
            OnActionResolved?.Invoke(SelectedCharacter, SelectedAction, SelectedTarget);
            
            var vm = combatManager.FindPartyMemberViewModel(SelectedCharacter);
            vm?.DisableSelection();
        }
        List<PartyMember> AlivePartyMembers = new List<PartyMember>();

        foreach (PartyMember mem in PartyMembers)
        {
            if (mem != null && mem.Alive) { AlivePartyMembers.Add(mem); }
        }
        if (AlivePartyMembers.All(m => !m.HasTurn))
        {
            bool extraTurns = false;
            foreach(PartyMember mem in AlivePartyMembers)
            {
                if (mem.gainExtraTurnNextRound)
                {
                    mem.HasTurn = true;
                    mem.gainExtraTurnNextRound = false;
                    extraTurns = true;
                }
            }
            if (!extraTurns)
            {
                ExecutePlayerActions();
            }
            
        }
            
        else
        {
            foreach (var button in combatManager.CharacterButtons)
            {

                button.SetActive(true);

            }
        }
        
    }

    public void ExecutePlayerActions()
    {
        Debug.Log("Turns in list: " + turns.Count);
        foreach (var t in turns)
        {
            if(t.Target.Count >=0)
            {
                Debug.Log("Targets for turn " + t.Target.Count);
            }
            if(t.Action.AbilityData.PhysicalBehaviour != null)
            {
                
                t.Action.AbilityData.PhysicalBehaviour.Execute(t.Attacker, t.Target, t.Action.AbilityData);
                
            }
            if (t.Action.AbilityData.EffectBehaviour != null)
            {
                t.Action.AbilityData.EffectBehaviour.Execute(t.Attacker, t.Target, t.Action.AbilityData);
                
            }
            if(t.Action.AbilityData.PhysicalBehaviour != null|| t.Action.AbilityData.EffectBehaviour != null)
            {
                t.Action.DecreaseUses();
            }
            t.Action.cooldownLeft = t.Action.AbilityData.cooldown;
           
        }
        bool hasImmediateExtras = false;
        foreach (PartyMember mem in PartyMembers)
        {
            if (mem.gainImmediateExtraTurn)
            {
                mem.HasTurn = true;
                mem.gainImmediateExtraTurn = false;
                hasImmediateExtras = true;
            }
            else
            {
                mem.gainImmediateExtraTurn = false;
                mem.HasTurn = false;
            }
        }

        if (hasImmediateExtras)
        {
            turns.Clear();
            StartCoroutine(DelayedPlayerPhaseStart());
        }
        else
        {
            EndPlayerPhase();
        }

    }

    public void StartPlayerPhase()
    {
        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = new List<CombatMember>() { };
        turns.Clear();

        foreach (PartyMember member in PartyMembers)
        {
            if (!member.IsStunned)
            {
                member.HasTurn = true;
            }


            //Go through all skills for all partymembers and decrease cooldown by 1
            foreach (var ability in member.activeAbilities)
            {
                if (ability != null)
                {
                    ability.DecreaseCooldown();
                }
            }
            
        }
 
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
        TurnOffAllHighlights();
        TurnOffAllCharacterSelecArrows();
        TurnOffAllTargetSelecArrows();
        CheckWinLoss();
        foreach (var member in PartyMembers)
        {
            member.TickEffects();
        }
        StartEnemyPhase();
    }

    private void StartEnemyPhase()
    {
        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = null;
        turns.Clear();

        foreach (var member in PartyMembers) member.HasTurn = false;

        OnEnemyPhaseStart?.Invoke();
    }

    public void ExecuteEnemyActions()
    {
        combatManager.enemyTurnManager.StartEnemyPhase();
    }

    public void EndEnemyPhase()
    {
        CheckWinLoss();
        foreach (var member in EnemyMembers)
        {
            member.TickEffects();
        }
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
    public void InvokeSelectingAction()
    {
        OnChoosingAction?.Invoke();
    }
    public void InvokeOnSelectingTarget()
    {
        OnSelectingTarget?.Invoke();
    }
    public void TurnOffAllHighlights()
    {
        foreach (var slot in combatManager.CharacterPositions)
        {
            slot.GetComponent<PartySlot>().TargetHighlight.SetActive(false);
        }
        foreach (var slot in combatManager.EnemyPositions)
        {
            slot.GetComponent<EnemySlot>().TargetHighlight.SetActive(false);
        }
    }

    public void TurnOffAllCharacterSelecArrows()
    {
        foreach (PartyMember mem in PartyMembers)
        {
            FindPartyMemberSlot(mem).GetComponent<PartySlot>().TurnCharacterArrowOff();
        }
    }
    public void TurnOffAllTargetSelecArrows()
    {
        foreach (PartyMember mem in PartyMembers)
        {
            FindPartyMemberSlot(mem).GetComponent<PartySlot>().TurnTargetArrowOff();
        }
        foreach (EnemyMember mem in EnemyMembers)
        {
            FindEnemyMemberSlot(mem).GetComponent<EnemySlot>().TurnTargetArrowOff();
        }
    }
}
