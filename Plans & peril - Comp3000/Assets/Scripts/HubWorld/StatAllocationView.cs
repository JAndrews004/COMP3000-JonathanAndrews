using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatAllocationView : MonoBehaviour
{
    
    public StatAllocationViewModel vm;
    public Image CharacterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLevel;
    public TextMeshProUGUI characterClass;
    public TextMeshProUGUI StatPointsAvailable;
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI DefenseStat;
    public TextMeshProUGUI IntelligenceStat;
    public TextMeshProUGUI MagicDefenseStat;
    public TextMeshProUGUI LuckStat;
    public TextMeshProUGUI HpStat;

    public Button CloseButton;
    public Button SaveButton;
    public Button InfoButton;
    public Button InfoExitButton;

    public GameObject InfoPannel;

    public Button DamagePlus;
    public Button Damageminus;
    public Button DefensePlus;
    public Button DefenseMinus;
    public Button IntelligencePlus;
    public Button IntelligenceMinus;
    public Button MagicDefensePlus;
    public Button MagicDefenseMinus;
    public Button LuckPlus;
    public Button LuckMinus;
    public Button HpPlus;
    public Button HpMinus;
    public void Bind(StatAllocationViewModel viewModel)
    {
        vm = viewModel;

        characterName.text = vm.character.baseStats.characterName;
        CharacterIcon.sprite = vm.character.baseStats.HeadShot;
        characterLevel.text = $"Level: {vm.character.level}";

        switch (vm.character.Class)
        {
            case CharacterClass.F:
                characterClass.text = $"Class: <color=#FFFFFF>F</color>";
                break;
            case CharacterClass.E:
                characterClass.text = $"Class: <color=#00FF00>E</color>";
                break;
            case CharacterClass.D:
                characterClass.text = $"Class: <color=#0000FF>D</color>";
                break;
            case CharacterClass.C:
                characterClass.text = $"Class: <color=#00FFFF>C</color>";
                break;
            case CharacterClass.B:
                characterClass.text = $"Class: <color=#800080>B</color>";
                break;
            case CharacterClass.A:
                characterClass.text = $"Class: <color=#ffa500>A</color>";
                break;
            case CharacterClass.S:
                characterClass.text = $"Class: <color=#FFFF00>S</color>";
                break;
        }

        StatPointsAvailable.text = $"Available stat points: <color=#00FF00>{vm.character.availableStatPoints}</color>";

        DamageStat.text = $"{vm.character.CurrentAttack} -> <color=#00FF00>{vm.character.CurrentAttack  + vm.DamangeIncrease}</color>";
        DefenseStat.text = $"{vm.character.CurrentDefense} -> <color=#00FF00>{vm.character.CurrentDefense  + vm.DefenseIncrease}</color>";
        IntelligenceStat.text = $"{vm.character.CurrentIntelligence} -> <color=#00FF00>{vm.character.CurrentIntelligence  + vm.IntelligenceIncrease}</color>";
        MagicDefenseStat.text = $"{vm.character.CurrentMagicDefense} -> <color=#00FF00>{vm.character.CurrentMagicDefense + vm.MagicDefenseIncrease}</color>";
        LuckStat.text = $"{vm.character.CurrentLuck} -> <color=#00FF00>{vm.character.CurrentLuck + 1 + vm.LuckIncrease}</color>";
        HpStat.text = $"{vm.character.CurrentMaxHealth / 10} -> <color=#00FF00>{vm.character.CurrentMaxHealth / 10  + vm.HpIncrease}</color>";

        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(() =>
        {
            vm.character.baseStats.avaliableStatPoints = vm.character.availableStatPoints;
            Destroy(this.gameObject);
        });
        SaveButton.onClick.RemoveAllListeners();
        SaveButton.onClick.AddListener(() =>
        {
            vm.character.baseStats.avaliableStatPoints = vm.character.availableStatPoints;
            vm.ApplyStatPoints();

        });

        InfoButton.onClick.RemoveAllListeners();
        InfoButton.onClick.AddListener(() =>
        {

            InfoPannel.SetActive(true);
        });
        InfoExitButton.onClick.RemoveAllListeners();
        InfoExitButton.onClick.AddListener(() =>
        {

            InfoPannel.SetActive(false);
        });

        Damageminus.interactable = false;
        DefenseMinus.interactable = false;
        IntelligenceMinus.interactable = false;
        MagicDefenseMinus.interactable = false;
        LuckMinus.interactable = false;
        HpMinus.interactable = false;


        DamagePlus.onClick.RemoveAllListeners();
        DamagePlus.onClick.AddListener(() =>
        {
            vm.DamangeIncrease += 1;
            vm.character.availableStatPoints -= 1;
            Damageminus.interactable = true;

        });
        DefensePlus.onClick.RemoveAllListeners();
        DefensePlus.onClick.AddListener(() =>
        {
            vm.DefenseIncrease += 1;
            vm.character.availableStatPoints -= 1;
            DefenseMinus.interactable = true;

        });
        IntelligencePlus.onClick.RemoveAllListeners();
        IntelligencePlus.onClick.AddListener(() =>
        {
            vm.IntelligenceIncrease += 1;
            vm.character.availableStatPoints -= 1;
            IntelligenceMinus.interactable = true;

        });
        MagicDefensePlus.onClick.RemoveAllListeners();
        MagicDefensePlus.onClick.AddListener(() =>
        {
            vm.MagicDefenseIncrease += 1;
            vm.character.availableStatPoints -= 1;
            MagicDefenseMinus.interactable = true;

        });
        LuckPlus.onClick.RemoveAllListeners();
        LuckPlus.onClick.AddListener(() =>
        {
            vm.LuckIncrease += 1;
            vm.character.availableStatPoints -= 1;
            LuckMinus.interactable = true;

        });

        HpPlus.onClick.RemoveAllListeners();
        HpPlus.onClick.AddListener(() =>
        {
            vm.HpIncrease += 1;
            vm.character.availableStatPoints -= 1;
            HpMinus.interactable = true;

        });

        Damageminus.onClick.RemoveAllListeners();
        Damageminus.onClick.AddListener(() =>
        {
            vm.DamangeIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
        DefenseMinus.onClick.RemoveAllListeners();
        DefenseMinus.onClick.AddListener(() =>
        {
            vm.DefenseIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
        IntelligenceMinus.onClick.RemoveAllListeners();
        IntelligenceMinus.onClick.AddListener(() =>
        {
            vm.IntelligenceIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
        MagicDefenseMinus.onClick.RemoveAllListeners();
        MagicDefenseMinus.onClick.AddListener(() =>
        {
            vm.MagicDefenseIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
        LuckMinus.onClick.RemoveAllListeners();
        LuckMinus.onClick.AddListener(() =>
        {
            vm.LuckIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
        HpMinus.onClick.RemoveAllListeners();
        HpMinus.onClick.AddListener(() =>
        {
            vm.HpIncrease -= 1;
            vm.character.availableStatPoints += 1;
            DamagePlus.interactable = true;
            DefensePlus.interactable = true;
            IntelligencePlus.interactable = true;
            MagicDefensePlus.interactable = true;
            LuckPlus.interactable = true;
            HpPlus.interactable = true;
        });
    }

    public void Update()
    {
        if (vm.character.availableStatPoints <= 0)
        {
            DamagePlus.interactable = false;
            DefensePlus.interactable = false;
            IntelligencePlus.interactable = false;
            MagicDefensePlus.interactable = false;
            LuckPlus.interactable = false;
            HpPlus.interactable = false;
        }


        if (vm.DamangeIncrease <= 0)
        {
            vm.DamangeIncrease = 0;
            Damageminus.interactable = false;
        }
        if (vm.DefenseIncrease <= 0)
        {
            vm.DefenseIncrease = 0;
            DefenseMinus.interactable = false;
        }
        if (vm.IntelligenceIncrease <= 0)
        {
            vm.IntelligenceIncrease = 0;
            IntelligenceMinus.interactable = false;
        }
        if (vm.MagicDefenseIncrease <= 0)
        {
            vm.MagicDefenseIncrease = 0;
            MagicDefenseMinus.interactable = false;
        }
        if (vm.LuckIncrease <= 0)
        {
            vm.LuckIncrease = 0;
            LuckMinus.interactable = false;
        }
        if (vm.HpIncrease <= 0)
        {
            vm.HpIncrease = 0;
            HpMinus.interactable = false;
        }

        StatPointsAvailable.text = $"Available stat points: <color=#00FF00>{vm.character.availableStatPoints}</color>";

        DamageStat.text = $"{vm.character.CurrentAttack} -> <color=#00FF00>{vm.character.CurrentAttack  + vm.DamangeIncrease}</color>";
        DefenseStat.text = $"{vm.character.CurrentDefense} -> <color=#00FF00>{vm.character.CurrentDefense  + vm.DefenseIncrease}</color>";
        IntelligenceStat.text = $"{vm.character.CurrentIntelligence} -> <color=#00FF00>{vm.character.CurrentIntelligence  + vm.IntelligenceIncrease}</color>";
        MagicDefenseStat.text = $"{vm.character.CurrentMagicDefense} -> <color=#00FF00>{vm.character.CurrentMagicDefense  + vm.MagicDefenseIncrease}</color>";
        LuckStat.text = $"{vm.character.CurrentLuck} -> <color=#00FF00>{vm.character.CurrentLuck + 1 + vm.LuckIncrease}</color>";
        HpStat.text = $"{vm.character.CurrentMaxHealth / 10} -> <color=#00FF00>{vm.character.CurrentMaxHealth / 10  + vm.HpIncrease}</color>";

    }
}
