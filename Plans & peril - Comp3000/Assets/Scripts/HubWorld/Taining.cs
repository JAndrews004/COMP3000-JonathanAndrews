using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Taining : MonoBehaviour
{
    public PartyMember chosenCharacter;
    public Button closeButton;

    public Button Character1;
    public Button Character2;
    public Button Character3;
    public Button Character4;

    public Button BasicTraining;
    public Button IntermediateTraining;
    public Button AdvancedTraining;

    public Button strengthTraining;
    public Button defenceTraining;
    public Button intelligenceTraining;
    public Button magicDefenceTraining;
    public Button luckTraining;
    public Button vitalityTraining;

    public Button InfoButton;
    public Button InfoExitButton;

    public GameObject InfoPannel;

    public TextMeshProUGUI CharacterSelection;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI goldBasicTrainingText;
    public TextMeshProUGUI goldInterTrainingText;
    public TextMeshProUGUI goldAdvTrainingText;

    public TextMeshProUGUI goldStrengthText;
    public TextMeshProUGUI goldDefenceText;
    public TextMeshProUGUI goldIntelligenceText;
    public TextMeshProUGUI goldMagicDefenceText;
    public TextMeshProUGUI goldLuckText;
    public TextMeshProUGUI goldVitalityText;

    private int goldCount;
    private int basicTrainingCost;
    private int intermediateTrainingCost;
    private int advancedTrainingCost;
    private int specialTrainingCost;

    private float basicFailureChance = 0.0f;
    private float intermediateFailureChance = 0.0f; 
    private float advancedFailureChance = 0.0f;

    private List<bool> characterTrained = new List<bool>{ false,false,false,false };
    private int indexOfCharacter = 0;
    // Start is called before the first frame update
    void Start()
    {
        chosenCharacter = GameManager.Instance.PartyMembers[0];
        basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
        intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
        advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
        specialTrainingCost = 500 * chosenCharacter.baseStats.level;
        updateFailures();
        updatePrices();
        goldCount = GameManager.Instance.GetGold();
        updateGold();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        Character1.onClick.RemoveAllListeners();
        Character1.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[0];
            indexOfCharacter = 0;
            basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
            intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
            advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
            specialTrainingCost = 500 * chosenCharacter.baseStats.level;
            updateFailures();
            updatePrices();
            updateSpecialButtons(0);
        });
        Character2.onClick.RemoveAllListeners();
        Character2.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[1];
            indexOfCharacter = 1;
            basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
            intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
            advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
            specialTrainingCost = 500 * chosenCharacter.baseStats.level;
            updateFailures();
            updatePrices();
            updateSpecialButtons(1);
        });
        Character3.onClick.RemoveAllListeners();
        Character3.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[2];
            indexOfCharacter = 2;
            basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
            intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
            advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
            specialTrainingCost = 500 * chosenCharacter.baseStats.level;
            updateFailures();
            updatePrices();
            updateSpecialButtons(2);
        });
        Character4.onClick.RemoveAllListeners();
        Character4.onClick.AddListener(() =>
        {
            chosenCharacter = GameManager.Instance.PartyMembers[3];
            indexOfCharacter = 3;
            basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
            intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
            advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
            specialTrainingCost = 500 * chosenCharacter.baseStats.level;
            updateFailures();
            updatePrices();
            updateSpecialButtons(3);
        });

        BasicTraining.onClick.RemoveAllListeners();
        BasicTraining.onClick.AddListener(() =>
        {
            if(chosenCharacter != null)
            {
                if(Random.Range(0, 1000) > basicFailureChance * 1000)
                {
                    basicTrainingCost = 100 * chosenCharacter.baseStats.level * 1;
                    if (basicTrainingCost <= goldCount)
                    {
                        TrainingForXp(chosenCharacter.baseStats.level, 0.05f,50);
                        GameManager.Instance.RemoveGold(basicTrainingCost);
                        updateGold();
                        updateFailures();
                        updatePrices();
                    }
                }
            }
        });
        IntermediateTraining.onClick.RemoveAllListeners();
        IntermediateTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (Random.Range(0, 1000) > intermediateFailureChance * 1000)
                {
                    intermediateTrainingCost = 100 * chosenCharacter.baseStats.level * 2;
                    if (intermediateTrainingCost <= goldCount && chosenCharacter.baseStats.level > 20)
                    {
                        TrainingForXp(chosenCharacter.baseStats.level, 0.1f, 50);
                        GameManager.Instance.RemoveGold(intermediateTrainingCost);
                        updateGold();
                        updateFailures();
                        updatePrices();
                    }
                }
            }
        });
        AdvancedTraining.onClick.RemoveAllListeners();
        AdvancedTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (Random.Range(0, 1000) > advancedFailureChance * 1000)
                {
                    advancedTrainingCost = 100 * chosenCharacter.baseStats.level * 4;
                    if (advancedTrainingCost <= goldCount && chosenCharacter.baseStats.level > 30)
                    {
                        TrainingForXp(chosenCharacter.baseStats.level, 0.15f, 50);
                        GameManager.Instance.RemoveGold(advancedTrainingCost);
                        updateGold();
                        updateFailures();
                        updatePrices();
                    }
                }
            }
        });
        strengthTraining.onClick.RemoveAllListeners();
        strengthTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.attack += 1;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }
                    
                }
            }
        });
        defenceTraining.onClick.RemoveAllListeners();
        defenceTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.defense += 1;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }

                }
            }
        });
        intelligenceTraining.onClick.RemoveAllListeners();
        intelligenceTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.intelligence += 1;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }

                }
            }
        });
        magicDefenceTraining.onClick.RemoveAllListeners();
        magicDefenceTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.magicDefence += 1;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }

                }
            }
        });
        luckTraining.onClick.RemoveAllListeners();
        luckTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.Luck += 1;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }

                }
            }
        });
        vitalityTraining.onClick.RemoveAllListeners();
        vitalityTraining.onClick.AddListener(() =>
        {
            if (chosenCharacter != null)
            {
                if (!characterTrained[indexOfCharacter])
                {
                    if (specialTrainingCost <= goldCount)
                    {
                        chosenCharacter.baseStats.maxHealth += 10;
                        characterTrained[indexOfCharacter] = true;
                        GameManager.Instance.RemoveGold(specialTrainingCost);
                        updateGold();
                        updatePrices();
                        updateSpecialButtons(indexOfCharacter);
                    }

                }
            }
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
    }

    void setAllSpecialedButtons(bool state)
    {
        strengthTraining.interactable = !state;
        defenceTraining.interactable = !state;
        intelligenceTraining.interactable = !state;
        magicDefenceTraining.interactable = !state;
        vitalityTraining.interactable= !state;
        luckTraining.interactable= !state;
    }
    void Update()
    {
       if(chosenCharacter != null)
        {
            CharacterSelection.text = chosenCharacter.baseStats.characterName;

            if(chosenCharacter.baseStats.level < 20)
            {
                BasicTraining.interactable = true;
                IntermediateTraining.interactable = false;
                AdvancedTraining.interactable = false;
            }
            else if (chosenCharacter.baseStats.level < 30)
            {
                BasicTraining.interactable = true;
                IntermediateTraining.interactable = true;
                AdvancedTraining.interactable = false;
            }
            else
            { 
                BasicTraining.interactable = true;
                IntermediateTraining.interactable = true;
                AdvancedTraining.interactable = true;
            }
        }
    }
    void updateSpecialButtons(int i)
    {
        if(i<0 || i >= 4)
        {
            return;
        }
        setAllSpecialedButtons(characterTrained[i]);
    }
    void TrainingForXp(int level,float percent, int baseXp)
    {
        int xpNext = 100*level*level;
        int gained = baseXp + Mathf.RoundToInt(percent * xpNext);

        if (chosenCharacter)
        {
            chosenCharacter.AddXP(gained);
        }
        updatePrices();
    }
    void updateGold()
    {
        goldText.text = $"Gold: {GameManager.Instance.GetGold()}";
    }

    void updatePrices()
    {
        goldBasicTrainingText.text = $"{basicTrainingCost}g ,{basicFailureChance*100}% failure";
        goldInterTrainingText.text = $"{intermediateTrainingCost}g ,{intermediateFailureChance * 100}% failure";
        goldAdvTrainingText.text = $"{advancedTrainingCost}g ,{advancedFailureChance * 100}% failure";
        goldStrengthText.text = $"{specialTrainingCost}g";
        goldDefenceText.text = $"{specialTrainingCost}g";
        goldIntelligenceText.text = $"{specialTrainingCost}g";
        goldMagicDefenceText.text = $"{specialTrainingCost}g";
        goldLuckText.text = $"{specialTrainingCost}g";
        goldVitalityText.text = $"{specialTrainingCost}g";
    }
    void updateFailures()
    {
        basicFailureChance = Mathf.Clamp(0.2f + (chosenCharacter.baseStats.level * 0.004f),0,0.7f);
        intermediateFailureChance = Mathf.Clamp(0.1f + (chosenCharacter.baseStats.level * 0.0025f),0,0.5f);
        advancedFailureChance = Mathf.Clamp(0.05f + (chosenCharacter.baseStats.level * 0.0015f),0,0.3f);
    }
}
