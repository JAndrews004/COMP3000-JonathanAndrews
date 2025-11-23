using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static AbilityData;
using static UnityEngine.GraphicsBuffer;

public class CombatManager : MonoBehaviour
{
    public List<PartySlot> CharacterPositions;
    public List<EnemySlot> EnemyPositions;
    public List<PartyMember> PartyMembers;

    public List<GameObject> CharacterButtons;
    public List<GameObject> EnemyTargetButtons;
    public List<GameObject> CharacterTargetButtons;

    [SerializeField] private GameObject partyMemberViewPrefab;
    [SerializeField] private Transform activeUnitUIParent;

    private TurnManager turnManager;
    public  EnemyTurnManager enemyTurnManager;
    public GameObject currentActiveView;
    public PartyMemberViewModel viewModel;

    public BattleLogManager battleLogManager;
    public StatusTooltip statusTooltip;

    public LevelUpManager levelUpManager;
    public CombatEndScreen combatEndScreen;

    private Dictionary<PartyMember, PartyMemberViewModel> partyMemberViewModels = new Dictionary<PartyMember, PartyMemberViewModel>();
    public List<PartyMemberViewModel> PartyMemberViewModels = new List<PartyMemberViewModel>();

    [HideInInspector] public bool win = false;
    void Start()
    {
        GameManager.Instance.RefreshPartyMembers();
        GameManager.Instance.RefreshEnemyMembers();
        PartyMembers = GameManager.Instance.PartyMembers;
        turnManager = GetComponentInChildren<TurnManager>();
        enemyTurnManager = GetComponentInChildren<EnemyTurnManager>();
        enemyTurnManager.tm = turnManager;
        for (int i =0; i < GameManager.Instance.EnemyMembers.Count;i++)
        {
            if (i >= 6)
            {
                Debug.Log("Too many enemies for positions (6)");
                break;
            }

            EnemyPositions[i].CurrentEnemyMember = GameManager.Instance.EnemyMembers[i];
        }

        // Subscribe to events
        turnManager.OnPlayerPhaseStart += PlayerPhaseStart;
        turnManager.OnEnemyPhaseStart += EnemyPhaseStart;
        turnManager.OnSelectingCharacter += SelectingCharacter;
        turnManager.OnChoosingAction += ChoosingAction;
        //turnManager.OnSelectingTarget += SelectingTarget;
        turnManager.OnConfirmRequired += ConfirmRequired;
        turnManager.OnActionResolved += ActionResolved;
        turnManager.OnCharacterSelected += ShowActiveUnitUI;

       
        StartCombat();
    }


    void OnDisable()
    {
        if (!turnManager) return;

        // Unsubscribe from events
        turnManager.OnPlayerPhaseStart -= PlayerPhaseStart;
        turnManager.OnEnemyPhaseStart -= EnemyPhaseStart;
        turnManager.OnSelectingCharacter -= SelectingCharacter;
        turnManager.OnChoosingAction -= ChoosingAction;
        //turnManager.OnSelectingTarget -= SelectingTarget;
        turnManager.OnConfirmRequired -= ConfirmRequired;
        turnManager.OnActionResolved -= ActionResolved;
        turnManager.OnCharacterSelected -= ShowActiveUnitUI;
    }

    public void StartCombat()
    {
        InitializePassives();
        for (int i = 0; i < PartyMembers.Count; i++)
        {
            CharacterPositions[i].GetComponent<PartySlot>().CurrentPartyMember = PartyMembers[i];
        }

        //GameManager.Instance.EnemyMembers = new List<EnemyMember> { EnemyPositions[0].GetComponent<EnemyMember>() };

        
        turnManager.StartCombat();
    }

    public void ShowActiveUnitUI(PartyMember partyMember)
    {
        if (!partyMember.Alive) return;

        if (currentActiveView != null)
            Destroy(currentActiveView);

        currentActiveView = Instantiate(partyMemberViewPrefab, activeUnitUIParent);

        var vm = new PartyMemberViewModel(partyMember, turnManager);
        currentActiveView.GetComponent<PartyMemberView>().Bind(vm);

        // Keep track of all ViewModels
        if (!PartyMemberViewModels.Contains(vm))
            PartyMemberViewModels.Add(vm);

        vm.EnableSelection();
        vm.StartChoosingAction();
    }


    public void HideActiveUnitUI()
    {
        if (currentActiveView != null)
        {
            Destroy(currentActiveView);
            currentActiveView = null;
        }
    }

    public void SelectingCharacter()
    {
        HideActiveUnitUI();
        foreach (var btn in CharacterButtons)
            btn.SetActive(true);
        foreach (var button in CharacterButtons)
        {
            var slot = button.GetComponentInParent<PartySlot>();
            var member = slot.CurrentPartyMember;

            bool canSelect = member != null && member.Alive && member.HasTurn;
            button.SetActive(canSelect);

            if (canSelect)
            {
                var btnComponent = button.GetComponent<Button>();
                btnComponent.onClick.RemoveAllListeners();
                PartyMember capturedMember = member;

                btnComponent.onClick.AddListener(() =>
                {
                    ShowActiveUnitUI(capturedMember);

                    // Enable selection for this character
                    var vm = FindPartyMemberViewModel(capturedMember);
                    if (vm != null)
                    {
                        vm.EnableSelection();   // allow attack / end turn buttons
                        vm.StartChoosingAction(); // show attack/end turn UI
                    }

                    // DO NOT hide buttons — allow player to switch
                });

            }
        }
    }

    public void DisableAllCharacterSelections()
    {
        foreach (var vm in PartyMemberViewModels)
        {
            vm.DisableSelection();
            //Debug.Log("Disabled selection");
            vm.DisableUI(); // optionally hide attack/end turn UI
        }
    }


    public void ChoosingAction()
    {
        //Debug.Log("Choosing an action!");
        viewModel?.StartChoosingAction();
    }

    /*
    public void SelectingTarget()
    {
        Debug.Log("Select a target!");

        foreach (var button in EnemyTargetButtons)
        {
            var slot = button.GetComponentInParent<EnemySlot>();
            var enemy = slot.CurrentEnemyMember;

            bool canSelect = enemy != null && enemy.Alive;
            button.SetActive(canSelect);

            if (canSelect)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                EnemyMember capturedEnemy = enemy;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // Set the selected target
                    turnManager.PlayerSelectedTarget(capturedEnemy);

                    // Update UI to highlight selection if you want
                    // Optional: you can keep all buttons active so the player can change selection
                });
            }
        }
    }*/

    public PartyMemberViewModel FindPartyMemberViewModel(PartyMember member)
    {
        if (partyMemberViewModels.TryGetValue(member, out var vm))
            return vm;
        return null;
    }

    public void ShowTargetButtons()
    {
        foreach(PartyMember pm in turnManager.PartyMembers)
        {
            turnManager.FindPartyMemberSlot(pm).GetComponent<PartySlot>().TurnTargetArrowOff();
        }
        foreach (EnemyMember em in turnManager.EnemyMembers)
        {
            turnManager.FindEnemyMemberSlot(em).GetComponent<EnemySlot>().TurnTargetArrowOff();
        }
        AbilityData currentAbility = turnManager.SelectedAction.AbilityData;
        turnManager.TurnOffAllHighlights();
        if (currentAbility == null)
        {
            return;
        }
        //Debug.Log("Showing target Buttons");

        switch (currentAbility.targetType)
            {
            case TargetType.SingleEnemy:
                //Debug.Log("Showing SingleEnemy Buttons");
                ShowEnemyTargetButtons();
                    break;

            case TargetType.MultipleEnemy:
                //Debug.Log("Showing MultipleEnemy Buttons");
                ShowEnemyTargetButtons();
                    break;
            case TargetType.AllEnemies:
                SelectAllEnemies();
                ConfirmRequired(turnManager.SelectedCharacter, turnManager.SelectedTarget);
                break;

            case TargetType.SingleAlly:
                //Debug.Log("Showing SingleAlly Buttons");
                ShowCharacterTargetButtons();
                    break;

            case TargetType.MultipleAlly:
                //Debug.Log("Showing MultipleAlly Buttons");
                ShowCharacterTargetButtons();
                    break;

            case TargetType.AllAllies:
                SelectAllAllies();
                ConfirmRequired(turnManager.SelectedCharacter, turnManager.SelectedTarget);
                break;
            case TargetType.None:
                //Debug.Log("Showing None Buttons");
                turnManager.SelectedTarget = new List<CombatMember> { turnManager.SelectedCharacter };
                ConfirmRequired(turnManager.SelectedCharacter, turnManager.SelectedTarget);
                    break;
            case TargetType.DeadAlly:
                ShowDeadAllyButtons();
                break;

            };
        
       
    }

    void ShowDeadAllyButtons()
    {
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();
        turnManager.SelectedTarget.Clear();
        foreach (var btn in CharacterButtons)
            btn.SetActive(false);

        //DisableAllCharacterSelections();
        foreach (var button in CharacterTargetButtons)
        {
            var slot = button.GetComponentInParent<PartySlot>();
            var partyMember = slot.CurrentPartyMember;
            bool canSelect = partyMember != null && !partyMember.Alive;
            button.SetActive(canSelect);
            slot.TargetHighlight.SetActive(canSelect);
            if (canSelect)
            {
                var btnComp = button.GetComponent<Button>();
                //Debug.Log("Selected: " + btnComp.name);
                btnComp.onClick.RemoveAllListeners();
                PartyMember capturedAlly = partyMember;
                btnComp.onClick.AddListener(() =>
                {
                    turnManager.PlayerSelectedTarget(capturedAlly);
                    // Optionally show confirm button for current character
                    //currentActiveView.GetComponent<PartyMemberView>().ShowConfirmButton();
                });
            }
        }
    }
    void SelectAllEnemies()
    {
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();
        turnManager.SelectedTarget.Clear();
        foreach(EnemyMember member in turnManager.EnemyMembers)
        {
            turnManager.SelectedTarget.Add(member); 
        }
    }
    void SelectAllAllies()
    {
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();
        turnManager.SelectedTarget.Clear();
        foreach (PartyMember member in turnManager.PartyMembers)
        {
            turnManager.SelectedTarget.Add(member);
        }
    }
    void ShowEnemyTargetButtons()
    {
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();
        turnManager.SelectedTarget.Clear();
        foreach (var button in EnemyTargetButtons)
        {
            var slot = button.GetComponentInParent<EnemySlot>();
            var enemy = slot.CurrentEnemyMember;
            bool canSelect = enemy != null && enemy.Alive;
            button.SetActive(canSelect);
            slot.TargetHighlight.SetActive(canSelect);
            if (canSelect)
            {
                var btnComp = button.GetComponent<Button>();
                btnComp.onClick.RemoveAllListeners();
                EnemyMember capturedEnemy = enemy;
                btnComp.onClick.AddListener(() =>
                {
                    turnManager.PlayerSelectedTarget(capturedEnemy);
                    // Optionally show confirm button for current character
                    //currentActiveView.GetComponent<PartyMemberView>().ShowConfirmButton();
                });
            }
        }
    }

    void ShowCharacterTargetButtons()
    {
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();
        turnManager.SelectedTarget.Clear();
        foreach (var btn in CharacterButtons)
            btn.SetActive(false);

        //DisableAllCharacterSelections();
        foreach (var button in CharacterTargetButtons)
        {
            var slot = button.GetComponentInParent<PartySlot>();
            var partyMember = slot.CurrentPartyMember;
            bool canSelect = partyMember != null && partyMember.Alive;
            button.SetActive(canSelect);
            slot.TargetHighlight.SetActive(canSelect);
            if (canSelect)
            {
                var btnComp = button.GetComponent<Button>();
                //Debug.Log("Selected: " + btnComp.name);
                btnComp.onClick.RemoveAllListeners();
                PartyMember capturedAlly = partyMember;
                btnComp.onClick.AddListener(() =>
                {
                    turnManager.PlayerSelectedTarget(capturedAlly);
                    // Optionally show confirm button for current character
                    //currentActiveView.GetComponent<PartyMemberView>().ShowConfirmButton();
                });
            }
        }
    }
    public void DisableAllTargetButtons()
    {
        foreach (var btn in CharacterTargetButtons)
        {
            btn.SetActive(false);
        }
        foreach(var btn in EnemyTargetButtons)
        {
            btn.SetActive(false);
        }
        foreach (var button in CharacterButtons)
        {

            button.SetActive(true);

        }
    }

    public void ConfirmRequired(PartyMember attacker, List<CombatMember> target)
    {
        //Debug.Log("Waiting for player to confirm attack.");
        currentActiveView?.GetComponent<PartyMemberView>().ShowConfirmButton();
        
    }

    public void ActionResolved(PartyMember attacker, Ability action, List<CombatMember> target)
    {
        //Debug.Log($"Action {action.abilityName} by {attacker.name} on {target.name} resolved.");
        HideActiveUnitUI();
    }

    public void PlayerPhaseStart()
    {
        //Debug.Log("Player Phase Started!");
        turnManager.StartCharacterSelection();

    }

    public void EnemyPhaseStart()
    {
        Debug.Log("Enemy Phase Started!");
        HideActiveUnitUI();
        turnManager.ExecuteEnemyActions();
    }

    public void OnConfirmButtonPressed()
    {
        turnManager.TurnOffAllHighlights();
        turnManager.TurnOffAllCharacterSelecArrows();
        turnManager.TurnOffAllTargetSelecArrows();
        turnManager.ConfirmAction();
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();

        // Hide all target buttons after confirmation
        foreach (var btn in EnemyTargetButtons) btn.SetActive(false);
        foreach (var btn in CharacterTargetButtons) btn.SetActive(false);
        
    }



    public void EndCombat()
    {
        RemoveAllPassives();
        // Hide any active UI
        HideActiveUnitUI();

        // Optionally disable all selection buttons
        foreach (var btn in CharacterButtons)
            btn.SetActive(false);

        foreach (var btn in EnemyTargetButtons)
            btn.SetActive(false);
        foreach (var btn in CharacterTargetButtons)
            btn.SetActive(false);
        // Any other cleanup, like resetting state or notifying GameManager
        Debug.Log("Combat ended!");

        
        combatEndScreen.Bind(PartyMembers, win);
        //Giving out XP to characters
        int xpchar1 = 0;
        int xpchar2 = 0;
        int xpchar3 = 0;
        int xpchar4 = 0;

        for (int i =0; i<turnManager.EnemyMembers.Count; i++)
        {
            var enemy = turnManager.EnemyMembers[i];
            int totalXp = enemy.XPGiven;
            int KillerBonus = Mathf.RoundToInt(totalXp * 0.3f);
            int remainingXp = totalXp - KillerBonus;
            float totalContributionPoints = 0;
            for (int j = 0; j < turnManager.PartyMembers.Count; j++)
            {
                totalContributionPoints += turnManager.PartyMembers[j].ContributionPoints;
            }

            for (int j=0; j < turnManager.PartyMembers.Count; j++)
            {
                float contributionPoints = turnManager.PartyMembers[j].ContributionPoints;

                int XpAward = Mathf.RoundToInt(contributionPoints / totalContributionPoints * remainingXp);
                if(enemy.PlayerKilledBy == turnManager.PartyMembers[j])
                {
                    XpAward += KillerBonus;
                }
                switch (j)
                {
                    case 0:
                        xpchar1 += XpAward;
                        break;
                    case 1:
                        xpchar2 += XpAward;
                        break;
                    case 2:
                        xpchar3 += XpAward;
                        break;
                    case 3:
                        xpchar4 += XpAward;
                        break;
                }
                turnManager.PartyMembers[j].AddXP(XpAward);
            }
        }
       
        StartCoroutine(AnimateXPBarsOnEnd(xpchar1, xpchar2, xpchar3, xpchar4));

        levelUpManager.StartLevelUpSequence();
        foreach(PartyMember mem in PartyMembers)
        {
            mem.baseStats.xp = mem.Xp;
        }
    }

    private void InitializePassives()
    {
        foreach (PartyMember member in GameManager.Instance.PartyMembers)
        {
            if (member != null)
                member.InitializePassives();
        }

        foreach (EnemyMember enemy in GameManager.Instance.EnemyMembers)
        {
            if (enemy != null)
                enemy.InitializePassives();
        }
    }

    private void RemoveAllPassives()
    {
        foreach (PartyMember member in GameManager.Instance.PartyMembers)
        {
            if (member != null)
                member.RemoveAllPassives();
        }

        foreach (EnemyMember enemy in GameManager.Instance.EnemyMembers)
        {
            if (enemy != null)
                enemy.RemoveAllPassives();
        }
    }

    public void RefreshAllStatusEffects()
    {
        foreach (var slot in CharacterPositions)
            slot.RefreshStatusEffects();

        foreach (var slot in EnemyPositions)
            slot.RefreshStatusEffects();
    }

    public IEnumerator AnimateXPBarsOnEnd(int x1, int x2, int x3, int x4 )
    {
        yield return combatEndScreen.UpdateXPChar1(PartyMembers[0], x1);
        yield return combatEndScreen.UpdateXPChar2(PartyMembers[1], x2);
        yield return combatEndScreen.UpdateXPChar3(PartyMembers[2], x3);
        yield return combatEndScreen.UpdateXPChar4(PartyMembers[3], x4);
    }
}
