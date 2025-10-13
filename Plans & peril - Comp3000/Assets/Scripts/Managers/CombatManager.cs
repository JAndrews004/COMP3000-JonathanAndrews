using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public List<GameObject> CharacterPositions;
    public List<EnemySlot> EnemyPositions;
    public List<PartyMember> PartyMembers;

    public List<GameObject> CharacterButtons;
    public List<GameObject> TargetButtons;

    [SerializeField] private GameObject partyMemberViewPrefab;
    [SerializeField] private Transform activeUnitUIParent;

    private TurnManager turnManager;
    public GameObject currentActiveView;
    public PartyMemberViewModel viewModel;

    private Dictionary<PartyMember, PartyMemberViewModel> partyMemberViewModels = new Dictionary<PartyMember, PartyMemberViewModel>();
    public List<PartyMemberViewModel> PartyMemberViewModels = new List<PartyMemberViewModel>();
    void Start()
    {
        GameManager.Instance.RefreshPartyMembers();
        GameManager.Instance.RefreshEnemyMembers();
        PartyMembers = GameManager.Instance.PartyMembers;
        turnManager = GetComponentInChildren<TurnManager>();

        for(int i =0; i < GameManager.Instance.EnemyMembers.Count;i++)
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
        turnManager.OnSelectingTarget += SelectingTarget;
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
        turnManager.OnSelectingTarget -= SelectingTarget;
        turnManager.OnConfirmRequired -= ConfirmRequired;
        turnManager.OnActionResolved -= ActionResolved;
        turnManager.OnCharacterSelected -= ShowActiveUnitUI;
    }

    public void StartCombat()
    {
        for (int i = 0; i < PartyMembers.Count; i++)
        {
            CharacterPositions[i].GetComponent<PartySlot>().CurrentPartyMember = PartyMembers[i];
        }

        GameManager.Instance.EnemyMembers = new List<EnemyMember> { EnemyPositions[0].GetComponent<EnemyMember>() };

        
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
            Debug.Log("Disabled selection");
            vm.DisableUI(); // optionally hide attack/end turn UI
        }
    }


    public void ChoosingAction()
    {
        Debug.Log("Choosing an action!");
        viewModel?.StartChoosingAction();
    }

    public void SelectingTarget()
    {
        Debug.Log("Select a target!");

        foreach (var button in TargetButtons)
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
    }

    public PartyMemberViewModel FindPartyMemberViewModel(PartyMember member)
    {
        if (partyMemberViewModels.TryGetValue(member, out var vm))
            return vm;
        return null;
    }

    public void ShowTargetButtons()
    {
        foreach (var button in TargetButtons)
        {
            var slot = button.GetComponentInParent<EnemySlot>();
            var enemy = slot.CurrentEnemyMember;
            bool canSelect = enemy != null && enemy.Alive;
            button.SetActive(canSelect);

            if (canSelect)
            {
                var btnComp = button.GetComponent<Button>();
                btnComp.onClick.RemoveAllListeners();
                EnemyMember capturedEnemy = enemy;
                btnComp.onClick.AddListener(() =>
                {
                    turnManager.PlayerSelectedTarget(capturedEnemy);
                    // Optionally show confirm button for current character
                    currentActiveView.GetComponent<PartyMemberView>().ShowConfirmButton();
                });
            }
        }
    }



    public void ConfirmRequired(PartyMember attacker, EnemyMember target)
    {
        Debug.Log("Waiting for player to confirm attack.");
        currentActiveView?.GetComponent<PartyMemberView>().ShowConfirmButton();
    }

    public void ActionResolved(PartyMember attacker, string action, EnemyMember target)
    {
        Debug.Log($"Action {action} by {attacker.name} on {target.name} resolved.");
        HideActiveUnitUI();
    }

    public void PlayerPhaseStart()
    {
        Debug.Log("Player Phase Started!");
        turnManager.StartCharacterSelection();

    }

    public void EnemyPhaseStart()
    {
        Debug.Log("Enemy Phase Started!");
        HideActiveUnitUI();
        StartCoroutine(ExecuteEnemyPhase());
    }

    private IEnumerator ExecuteEnemyPhase()
    {
        yield return new WaitForSeconds(1f);
        turnManager.ExecuteEnemyActions();
    }

    public void OnConfirmButtonPressed()
    {
        turnManager.ConfirmAction();
        currentActiveView?.GetComponent<PartyMemberView>().HideConfirmButton();

        // Hide all target buttons after confirmation
        foreach (var btn in TargetButtons) btn.SetActive(false);

    }



    public void EndCombat()
    {
        // Hide any active UI
        HideActiveUnitUI();

        // Optionally disable all selection buttons
        foreach (var btn in CharacterButtons)
            btn.SetActive(false);

        foreach (var btn in TargetButtons)
            btn.SetActive(false);

        // Any other cleanup, like resetting state or notifying GameManager
        Debug.Log("Combat ended!");
    }

}
