using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PartyMemberViewModel;

public class PartyMemberView : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Button attackButton;
    public Button confirmButton;
    public Button endTurnButton;

    public PartyMemberViewModel viewModel;

    public void Bind(PartyMemberViewModel vm)
    {
        viewModel = vm;

        // Initial display
        nameText.text = vm.model.baseStats.characterName;
        hpSlider.maxValue = vm.model.CurrentMaxHealth;
        hpSlider.value = vm.model.CurrentHealth;

        Debug.Log($"Binding buttons for {vm.model.baseStats.characterName}");
        // Button bindings
        attackButton.onClick.AddListener(() => {
            vm.AttackButtonPressed();
            //Debug.Log("Attack button clicked!");
            
        });

        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(() => {
            //Debug.Log("End Turn button clicked!");
            vm.EndTurnButtonPressed();
        });

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => vm.OnConfirmButtonPressed());

        // Events
        vm.OnUIStateChanged += UpdateUIState;
        vm.OnHealthChanged += UpdateHealthBar;
        vm.OnTurnStateChanged += UpdateButtons;
    }

    private void UpdateHealthBar(int newHealth) => hpSlider.value = newHealth;

    private void UpdateButtons(bool canAct)
    {
        attackButton.interactable = canAct;
        endTurnButton.interactable = canAct;
    }

    private void UpdateUIState(PartyUIState state)
    {
        attackButton.gameObject.SetActive(state == PartyUIState.ChoosingAction);
        endTurnButton.gameObject.SetActive(state == PartyUIState.ChoosingAction);
        confirmButton.gameObject.SetActive(state == PartyUIState.Confirm);
    }

    public void ShowConfirmButton() => confirmButton.gameObject.SetActive(true);
    public void HideConfirmButton() => confirmButton.gameObject.SetActive(false);

    private void OnDestroy()
    {
        if (viewModel == null) return;

        viewModel.OnUIStateChanged -= UpdateUIState;
        viewModel.OnHealthChanged -= UpdateHealthBar;
        viewModel.OnTurnStateChanged -= UpdateButtons;
    }

}
