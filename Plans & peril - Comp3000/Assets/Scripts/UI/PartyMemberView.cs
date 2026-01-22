using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PartyMemberViewModel;

public class PartyMemberView : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public TextMeshProUGUI healthText;
    public Image element;

    public Button[] abilityButtons;

    public TextMeshProUGUI[] abilityButtonsText;
    public TextMeshProUGUI[] usagesText;

    public Button confirmButton;
    public Button endTurnButton;
    public Button clearButton;

    public PartyMemberViewModel viewModel;

    public void Bind(PartyMemberViewModel vm)
    {

        viewModel = vm;

        // Initial display
        nameText.text = vm.model.baseStats.characterName;
        hpSlider.maxValue = vm.model.CurrentMaxHealth;
        hpSlider.value = vm.model.CurrentHealth;
        element.sprite = vm.model.baseStats.EquippedElement.icon;
        for (int i = 0;i< vm.model.activeAbilities.Count; i++)
        {
            if (vm.model.activeAbilities[i].AbilityData != null)
            {
                abilityButtonsText[i].text = vm.model.activeAbilities[i].AbilityData.abilityName ?? null;
                abilityButtons[i].interactable = true;
            }
         
        }

        for (int i = 0; i < vm.model.activeAbilities.Count; i++)
        {
            if (vm.model.activeAbilities[i] != null)
            {
                usagesText[i].text = $"{vm.model.activeAbilities[i].usesLeft}/{vm.model.activeAbilities[i].AbilityData.maxUsage}";
            }
            
        }

        healthText.text = $"{ vm.model.CurrentHealth.ToString()}/{vm.model.CurrentMaxHealth.ToString()}";

        for (int i = 0; i < vm.model.activeAbilities.Count && i < abilityButtons.Length; i++)
        {
            Ability ability = vm.model.activeAbilities[i];

            // Hook up ability click
            abilityButtons[i].onClick.RemoveAllListeners();
            abilityButtons[i].onClick.AddListener(() =>
            {
                vm.AbilityButtonPressed(ability);
                
            });
        }
        for(int i= 0; i< vm.model.passiveAbilities.Count; i++)
        {
            Ability ability = vm.model.passiveAbilities[i];
            abilityButtonsText[i+ vm.model.activeAbilities.Count].text = vm.model.passiveAbilities[i].AbilityData.abilityName ?? null;
            abilityButtons[i+ vm.model.activeAbilities.Count].interactable = false;

            usagesText[i+vm.model.activeAbilities.Count].text = "";
        }

        for(int i = 0; i < vm.model.activeAbilities.Count; i++)
        {
            if (vm.model.activeAbilities[i] != null && vm.model.activeAbilities[i].usesLeft <= 0 )
            {
                //Debug.Log("Setting interactability to false for button " + i);
                abilityButtons[i].interactable = false;
            }
        }
        for (int i = 0; i < vm.model.activeAbilities.Count; i++)
        {
            if (vm.model.activeAbilities[i] != null && vm.model.activeAbilities[i].cooldownLeft > 0 )
            {
                Debug.Log("On Cooldown: button " + i);
                abilityButtons[i].interactable = false;
            }
        }
        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(() => {
            //Debug.Log("End Turn button clicked!");
            vm.EndTurnButtonPressed();
        });
        clearButton.onClick.RemoveAllListeners();
        clearButton.onClick.AddListener(() => {
            //Debug.Log("End Turn button clicked!");
            vm.clearButtonPressed();
        });

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => vm.OnConfirmButtonPressed());

        // Events
        vm.OnUIStateChanged += UpdateUIState;
        vm.OnHealthChanged += UpdateHealthBar;
        vm.OnTurnStateChanged += UpdateButtons;
        
    }

    private void UpdateHealthBar(int newHealth)
    {
        healthText.text = $"{newHealth}/{hpSlider.maxValue}";
        hpSlider.value = newHealth;
        
    
    }

    private void UpdateButtons(bool canAct)
    { 
        endTurnButton.interactable = canAct;
    }

    private void UpdateUIState(PartyUIState state)
    {
        //abilityButton1.gameObject.SetActive(state == PartyUIState.ChoosingAction);
        //endTurnButton.gameObject.SetActive(state == PartyUIState.ChoosingAction);
        confirmButton.gameObject.SetActive(state == PartyUIState.Confirm);
        if(state == PartyUIState.SelectingTarget)
        {
            confirmButton.gameObject.SetActive(false);
        }
        
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
