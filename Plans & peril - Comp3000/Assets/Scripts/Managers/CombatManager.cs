using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public List<GameObject> CharacterPositions;
    public List<GameObject> EnemyPositions;
    public List<PartyMember> PartyMembers;

    [SerializeField] private GameObject partyMemberViewPrefab;
    [SerializeField] private Transform activeUnitUIParent;

    private TurnManager turnManager;
    private GameObject currentActiveView;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RefreshPartyMembers();
        PartyMembers = GameManager.Instance.PartyMembers;
        turnManager = gameObject.GetComponentInChildren<TurnManager>();

        foreach(GameObject Enemy in EnemyPositions)
        {
            Enemy.GetComponent<EnemyMember>().CurrentHealth = Enemy.GetComponent<EnemyMember>().CurrentMaxHealth;
        }

        StartCombat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCombat()
    {
        for (int i = 0; i < PartyMembers.Count; i++)
        {
            CharacterPositions[i].GetComponent<PartySlot>().CurrentPartyMember = PartyMembers[i];
        }

        GameManager.Instance.EnemyMembers =new List<EnemyMember> { EnemyPositions[0].GetComponent<EnemyMember>() };

        turnManager.StartCombat();
    }

    public void ShowActiveUnitUI(PartyMember partyMember)
    {
        
        if (currentActiveView != null)
            Destroy(currentActiveView);

        
        currentActiveView = Instantiate(partyMemberViewPrefab, activeUnitUIParent);

        
        var vm = new PartyMemberViewModel(partyMember, turnManager);


        currentActiveView.GetComponent<PartyMemberView>().Bind(vm);
        vm.StartTurn();

    }
    public void HideActiveUnitUI()
    {
        if (currentActiveView != null)
        {
            Destroy(currentActiveView);
            currentActiveView = null;
        }
    }
}
