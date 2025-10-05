using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberView : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Button attackButton;
    public Button endTurnButton;

    private PartyMemberViewModel viewModel;

    public void Bind(PartyMemberViewModel vm)
    {
        viewModel = vm;

        // Set initial display
        nameText.text = vm.model.baseStats.characterName;
        hpSlider.maxValue = vm.model.CurrentMaxHealth;
        hpSlider.value = vm.model.CurrentHealth;

        // Bind buttons with lambdas
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => viewModel.AttackButtonPressed());

        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(() => viewModel.EndTurnButtonPressed());

        // Subscribe to events
        vm.OnHealthChanged += UpdateHealthBar;
        vm.OnTurnStateChanged += UpdateButtons;
    }


    private void UpdateHealthBar(int newHealth)
    {
        hpSlider.value = newHealth;
    }

    private void UpdateButtons(bool canAct)
    {
        attackButton.interactable = canAct;
        endTurnButton.interactable = canAct;
    }
}
