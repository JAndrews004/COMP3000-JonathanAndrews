using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    public bool playerPhase = true;
    public debugCombatLog DebugCombatLog = new debugCombatLog();
    public DebugManager debugManager;
    public void StartCombat()
    {
        //Debug.Log("Starting Combat from TM");
        combatManager = GetComponentInParent<CombatManager>();
        PartyMembers = GameManager.Instance.PartyMembers;
        foreach (PartyMember p in PartyMembers)
        {
            p.combatManager = combatManager;
        }
        foreach(var Enemyslot in combatManager.EnemyPositions)
        {
            if (Enemyslot.CurrentEnemyMember != null)
            {
                EnemyMembers.Add(Enemyslot.CurrentEnemyMember);
                Enemyslot.CurrentEnemyMember.combatManager = combatManager;
            }
        }
        combatManager.enemyTurnManager.RegisterEnemies(EnemyMembers);
        GameManager.Instance.EnemyMembers = EnemyMembers;

        DebugCombatLog = new debugCombatLog();
        DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };
        DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
        DebugCombatLog.targets = new List<CombatMember> { };

        foreach (CombatMember member in PartyMembers)
        {
            member.IsStunned = false;
        }

        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager)
        {
            GameManager.Instance.tutorialManager.setUpTutorialData(this);
        }

        

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

    public PartySlot FindPartyMemberSlot(CombatMember member)
    {
        PartySlot selectedSlot = null;

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
        DebugCombatLog.abilityUsed = action;
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
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState != TutorialState.SelectTarget)
        {
            return;
        }

        if (SelectedTarget == null) { 
            SelectedTarget = new List<CombatMember>();
        }
        
        int addedTargets = 0;
        if ((SelectedCharacter.element == SelectedAction.AbilityData.elementTag && SelectedAction.AbilityData.elementTag != Element.None && SelectedAction.AbilityData.boost != null))
        {
            addedTargets = SelectedAction.AbilityData.boost.additionalTargets;
        }
        if (SelectedTarget.Contains(target))
        {
            SelectedTarget.Remove(target);
            if (target is PartyMember)
            {
                FindPartyMemberSlot(target).GetComponent<PartySlot>().TurnTargetArrowOff();
            }
            else
            {
                FindEnemyMemberSlot(target).GetComponent<EnemySlot>().TurnTargetArrowOff();
            }
        }
        
        else if (SelectedTarget.Count() < SelectedAction.AbilityData.numberOfTargets+ addedTargets)
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

        GameManager.Instance.tutorialManager.currentState = TutorialState.EndTurn;
    }



    public void ConfirmAction()
    {
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState != TutorialState.EndTurn)
        {
            return;
        }

        if (SelectedCharacter != null && SelectedCharacter.HasTurn &&
            SelectedAction != null && SelectedTarget != null && SelectedTarget.Count() >0)
        {
            
            Turn newturn = new Turn(SelectedTarget, SelectedAction, SelectedCharacter);
            turns.Add(newturn);
            SelectedCharacter.HasTurn = false;

           
            OnActionResolved?.Invoke(SelectedCharacter, SelectedAction, SelectedTarget);

            string targetNames = string.Join(", ", newturn.Target.Select(t => $"<color=#FF0000>{t.baseStats.characterName}</color>"));

            if(newturn.Target != null)
            {
                if (newturn.Target[0] is PartyMember)
                {
                    targetNames = string.Join(", ", newturn.Target.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"));
                }
            }
            if(combatManager.battleLogManager != null)
            combatManager.battleLogManager.AddMessage(
                $"<color=#00FF00>{newturn.Attacker.baseStats.characterName}</color> " +
                $"used <color=#0000FF>{newturn.Action.AbilityData.abilityName}</color> on {targetNames}"
            );
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
                StartCoroutine(ExecutePlayerActionsRoutine());

            }

        }
            
        else
        {
            foreach (var button in combatManager.CharacterButtons)
            {

                button.SetActive(true);

            }
        }

        GameManager.Instance.tutorialManager.turnCounter++;
        if(GameManager.Instance.tutorialManager.turnCounter < 3 && GameManager.Instance.tutorialActive)
        {
            GameManager.Instance.tutorialManager.currentState = TutorialState.SelectAbility;
        }
        else if(GameManager.Instance.tutorialActive)
        {
            TriggerForceLoss();
        }
    }

    public IEnumerator ExecutePlayerActionsRoutine()
    {
        Debug.Log("Turns in list: " + turns.Count);
        foreach (var t in turns)
        {
            
            if (t.Action.AbilityData.IsTauntable)
            {
                yield return new WaitForSeconds(0.5f);
                List<CombatMember> newTargets = new List<CombatMember>();
                List<CombatMember> Taunters = new List<CombatMember>();

                foreach (Effect effect in t.Attacker.activeEffects)
                {
                    if (effect is TauntEffect)
                    {
                 
                        Taunters.Add(effect.User);
                    }
                }
                foreach (CombatMember Taunt in Taunters)
                {
                    if (newTargets.Count == t.Target.Count)
                    {
                        break;
                    }
                    newTargets.Add(Taunt);
                }

                foreach (CombatMember Taunt in t.Target)
                {
                    if (newTargets.Count == t.Target.Count)
                    {
                        break;
                    }
                    if (!newTargets.Contains(Taunt))
                    {
                        newTargets.Add(Taunt);
                    }
                }
                if(Taunters.Count >= 1)
                {
                    combatManager.battleLogManager.AddMessage(
                    $"<color=#00FF00>{t.Attacker.baseStats.characterName}</color> " +
                    $"was prevoked by {string.Join(", ", Taunters.Select(t => $"<color=#00FF00>{t.baseStats.characterName}</color>"))} and used <color=#0000FF>{t.Action.AbilityData.abilityName}</color>"
                    );
                }
                
                t.Target = newTargets;
            }
            if (t.Action.AbilityData.PhysicalBehaviour != null)
            {
                
                t.Action.AbilityData.PhysicalBehaviour.Execute(t.Attacker, t.Target, t.Action.AbilityData);
                
            }
            if (t.Action.AbilityData.EffectBehaviour != null)
            {
                t.Action.AbilityData.EffectBehaviour.Execute(t.Attacker, t.Target, t.Action.AbilityData);
                
            }
            if(t.Action.AbilityData.PhysicalBehaviour != null|| t.Action.AbilityData.EffectBehaviour != null)
            {
                t.Action.DecreaseUses(t.Attacker);
            }
            t.Action.cooldownLeft = t.Action.AbilityData.cooldown;
            debugManager.setDebugLogText(DebugCombatLog);
            yield return new WaitForSeconds(0.5f);
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
        DebugCombatLog = new debugCombatLog();
        DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };
        DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
        DebugCombatLog.targets = new List<CombatMember> { };

        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = new List<CombatMember>() { };
        turns.Clear();
        playerPhase = true;
        bool canGo = false;
        foreach (PartyMember member in PartyMembers)
        {
            if (!member.IsStunned)
            {
                member.HasTurn = true;
                foreach(Effect effect in member.activeEffects)
                {
                    if(effect.statusEffectType == StatusEffect.Delay)
                    {
                        member.HasTurn = false;
                    }
                }
                
            }

            //Go through all skills for all partymembers and decrease cooldown by 1
            foreach (var ability in member.activeAbilities)
            {
                if (ability != null)
                {
                    ability.DecreaseCooldown();
                }
            }
            if (member.HasTurn && member.Alive)
            {
                canGo = true;
            }
        }

        if (!canGo)
        {
            StartEnemyPhase();
            return;
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
        
        foreach (var member in PartyMembers)
        {
            member.TickEffects();
            member.damageReceivedPrieviousRound = 0;
        }
        combatManager.RefreshAllStatusEffects();

        if (!CheckWinLoss())
        {
            StartEnemyPhase();
        }
        DebugCombatLog = new debugCombatLog();
        DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };

        DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
        DebugCombatLog.targets = new List<CombatMember> { };
    }

    private void StartEnemyPhase()
    {
        DebugCombatLog = new debugCombatLog();
        DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };

        DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
        DebugCombatLog.targets = new List<CombatMember> { };

        SelectedCharacter = null;
        SelectedAction = null;
        SelectedTarget = null;
        turns.Clear();
        playerPhase = false;
        foreach (var member in PartyMembers) member.HasTurn = false;

        OnEnemyPhaseStart?.Invoke();
    }

    public void ExecuteEnemyActions()
    {
        combatManager.enemyTurnManager.StartEnemyPhase();
    }

    public void EndEnemyPhase()
    {
        
        foreach (var member in EnemyMembers)
        {
            member.TickEffects();
        }
        combatManager.RefreshAllStatusEffects();
        if (!CheckWinLoss())
        {
            StartPlayerPhase();
        }
        DebugCombatLog = new debugCombatLog();
        DebugCombatLog.rawDamages = new Dictionary<CombatMember, int> { };
        DebugCombatLog.damageReceived = new Dictionary<CombatMember, int> { };
        DebugCombatLog.targets = new List<CombatMember> { };

    }

    public bool CheckWinLoss()
    {
        if (EnemyMembers.All(e => !e.Alive))
        {
            Debug.Log("All enemies defeated! You win!");
            combatManager.win = true;
            combatManager.EndCombat();
            
            return true;
        }
        else if (PartyMembers.All(p => !p.Alive))
        {
            Debug.Log("All players defeated! Game over!");
            combatManager.win = false;
            combatManager.EndCombat();
            return true;
        }
        return false;
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
            if(slot.GetComponent<PartySlot>().TargetHighlight!=null)
            slot.GetComponent<PartySlot>().TargetHighlight.SetActive(false);
        }
        foreach (var slot in combatManager.EnemyPositions)
        {
            if (slot.GetComponent<EnemySlot>().TargetHighlight != null)
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
    public void TriggerForceLoss()
    {
        foreach(PartyMember mem in PartyMembers)
        {
            mem.CurrentHealth = 0;
        }
        GameManager.Instance.tutorialManager.unsubscribeToEvents();
        CheckWinLoss();
    }
}
