using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsMenu : MonoBehaviour
{
    
    public PartyMember chosenCharacter;
    public Button closeButton;

    public Button Character1;
    public Button Character2;
    public Button Character3;
    public Button Character4;

    public Button statPointAllocation;

    public GameObject statPointScreen;

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
    public SkillTreeUIContoller skillTreeUIContoller;
    
    public StatAllocationViewModel StatAllocationViewModel;
    public GameObject StatAllocationPrefab;
    // Start is called before the first frame update
    void Start()
    {
        chosenCharacter = GameManager.Instance.PartyMembers[0];
        newCharacterSelected();
        
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        Character1.onClick.RemoveAllListeners();
        Character1.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[0];
            newCharacterSelected();
            skillTreeUIContoller.generateSkillTree(chosenCharacter);

        });
        Character2.onClick.RemoveAllListeners();
        Character2.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[1];
            newCharacterSelected();
            skillTreeUIContoller.generateSkillTree(chosenCharacter);

        });
        Character3.onClick.RemoveAllListeners();
        Character3.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[2];
            newCharacterSelected();
            skillTreeUIContoller.generateSkillTree(chosenCharacter);


        });
        Character4.onClick.RemoveAllListeners();
        Character4.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[3];
            newCharacterSelected();
            skillTreeUIContoller.generateSkillTree(chosenCharacter);

        });
        statPointAllocation.onClick.RemoveAllListeners();
        statPointAllocation.onClick.AddListener(() =>
        {
            
            GameObject levelUpPrefab = Instantiate(StatAllocationPrefab, this.transform);
            StatAllocationViewModel = new StatAllocationViewModel(chosenCharacter, levelUpPrefab);
            levelUpPrefab.GetComponent<StatAllocationView>().Bind(StatAllocationViewModel);
        });

        if (GameManager.Instance.tutorialActive)
        {
            Character2.interactable = false;
            Character3.interactable = false;
            Character4.interactable = false;
            statPointAllocation.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateStats()
    {
        StatPointsAvailable.text = $"Available stat points: <color=#00FF00>{chosenCharacter.availableStatPoints}</color>";

        DamageStat.text = $"STR:{chosenCharacter.CurrentAttack}";
        DefenseStat.text = $"DEF:{chosenCharacter.CurrentDefense}";
        IntelligenceStat.text = $"INT:{chosenCharacter.CurrentIntelligence}";
        MagicDefenseStat.text = $"MDF:{chosenCharacter.CurrentMagicDefense}";
        LuckStat.text = $"LCK:{chosenCharacter.CurrentLuck}";
        HpStat.text = $"VIT:{chosenCharacter.CurrentMaxHealth / 10}";
    }
    void newCharacterSelected()
    {
        updateStats();
        skillTreeUIContoller.generateSkillTree(chosenCharacter);
        characterLevel.text = $"Level: {chosenCharacter.level}";
        characterName.text = chosenCharacter.baseStats.name;

        switch (chosenCharacter.Class)
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
    }
}
