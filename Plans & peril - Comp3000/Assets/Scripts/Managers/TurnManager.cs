using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers = new List<EnemyMember>();
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
        StartPlayerPhase();
    }

    public void SetSelectedCharacter(PartyMember member)
    {

        //go through enemy members and see if curentPartyMember = SelectedCharacter

        if (SelectedCharacter != null)
        {
            FindPartyMemberSlot(member).GetComponent<PartySlot>().TargetHighlight.SetActive(false);
        }
        
        SelectedCharacter = member;

        FindPartyMemberSlot(member).GetComponent<PartySlot>().TargetHighlight.SetActive(true);

        OnCharacterSelected?.Invoke(member);
        OnChoosingAction?.Invoke();
    }

    public GameObject FindPartyMemberSlot(PartyMember member)
    {
        GameObject selectedSlot = null;

        if (SelectedCharacter != null)
        {
            for (int i = 0; i < combatManager.CharacterPositions.Count; i++)
            {
                if (PartyMembers[i] == SelectedCharacter)
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

    public EnemySlot FindEnemyMemberSlot(EnemyMember member)
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

        
        Debug.Log("Before click: " + SelectedTarget.Count());
        

        if (SelectedTarget.Contains(target))
        {
            SelectedTarget.Remove(target);
        }
        else if (SelectedTarget.Count() < SelectedAction.AbilityData.numberOfTargets)
        {
            SelectedTarget.Add(target);
        }

        Debug.Log("Adding " + target.name);
        Debug.Log("After click: " + SelectedTarget.Count());

       // FindEnemyMemberSlot(SelectedTarget).GetComponent<EnemySlot>().TargetHighlight.SetActive(true);
        

        if(SelectedAction.AbilityData.numberOfTargets == SelectedTarget.Count)
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
            turns.Add(new Turn(SelectedTarget, SelectedAction, SelectedCharacter));
            SelectedCharacter.HasTurn = false;

            FindPartyMemberSlot(SelectedCharacter).GetComponent<PartySlot>().TargetHighlight.SetActive(false);
            //FindEnemyMemberSlot(SelectedTarget).GetComponent<EnemySlot>().TargetHighlight.SetActive(false);

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
            ExecutePlayerActions();
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
        foreach (var t in turns)
        {
            t.Action.AbilityData.behaviour.Execute(t.Attacker, t.Target );
            t.Action.DecreaseUses();

            t.Action.cooldownLeft = t.Action.AbilityData.cooldown;
           
        }
        EndPlayerPhase();
    }

    public void StartPlayerPhase()
    {
        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = null;
        turns.Clear();

        foreach (var member in PartyMembers)
        {
            if (!member.IsStunned)
            {
                member.HasTurn = true;
            }


            //Go through all skills for all partymembers and decrease cooldown by 1
            foreach (var ability in member.abilities)
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
        foreach(var slot in combatManager.CharacterPositions)
        {
            slot.GetComponent<PartySlot>().TargetHighlight.SetActive(false);
        }
        foreach (var slot in combatManager.EnemyPositions)
        {
            slot.GetComponent<EnemySlot>().TargetHighlight.SetActive(false);
        }
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
        /*
        foreach (var enemy in EnemyMembers.Where(e => e.Alive))
        {
            var target = PartyMembers.FirstOrDefault(p => p.Alive);
            if (target != null) enemy.BasicAttack(target);

            Debug.Log($"{enemy.baseStats.characterName} attacked {target.baseStats.characterName} for {enemy.CurrentAttack} damage");
        }*/

        combatManager.enemyTurnManager.StartEnemyPhase();
        EndEnemyPhase();
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

}
