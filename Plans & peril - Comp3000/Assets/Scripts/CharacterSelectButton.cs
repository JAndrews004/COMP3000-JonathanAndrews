using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    private TurnManager turnManager;
    private PartySlot parentSlot;
    private Button button;

    void Awake()
    {
        parentSlot = GetComponentInParent<PartySlot>();
        turnManager = FindObjectOfType<TurnManager>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        // Disable button if character has already had their turn
        if (parentSlot?.CurrentPartyMember != null)
            button.interactable = parentSlot.CurrentPartyMember.HasTurn;
    }

    private void OnClick()
    {
        var member = parentSlot?.CurrentPartyMember;
        if (member == null) return;

        Debug.Log($"Clicked character: {member.baseStats.characterName}");
        turnManager.SetSelectedCharacter(member);
    }
}
