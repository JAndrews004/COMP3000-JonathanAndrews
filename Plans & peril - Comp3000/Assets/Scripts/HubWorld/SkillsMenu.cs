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


        });
        Character2.onClick.RemoveAllListeners();
        Character2.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[1];
            newCharacterSelected();


        });
        Character3.onClick.RemoveAllListeners();
        Character3.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[2];
            newCharacterSelected();



        });
        Character4.onClick.RemoveAllListeners();
        Character4.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[3];
            newCharacterSelected();


        });
        statPointAllocation.onClick.RemoveAllListeners();
        statPointAllocation.onClick.AddListener(() =>
        {
            
        });
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
