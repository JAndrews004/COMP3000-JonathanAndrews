using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button closeButton;
    public Button resetStatPointsChar1;
    public Button resetStatPointsChar2;
    public Button resetStatPointsChar3;
    public Button resetStatPointsChar4;
    public Button upgradeDyngeonPass;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI upgradeDyngeonPassgoldText;
    public TextMeshProUGUI resetStatPointsChar1goldText;
    public TextMeshProUGUI resetStatPointsChar2goldText;
    public TextMeshProUGUI resetStatPointsChar3goldText;
    public TextMeshProUGUI resetStatPointsChar4goldText;

    int resetStatPointsChar1gold = 0;
    int resetStatPointsChar2gold = 0;
    int resetStatPointsChar3gold = 0;
    int resetStatPointsChar4gold = 0;

    int goldCount;
    // Start is called before the first frame update
    void Start()
    {
        resetStatPointsChar1gold = GameManager.Instance.PartyMembers[0].baseStats.level * 500 + 2000;
        resetStatPointsChar2gold = GameManager.Instance.PartyMembers[1].baseStats.level * 500 + 2000;
        resetStatPointsChar3gold = GameManager.Instance.PartyMembers[2].baseStats.level * 500 + 2000;
        resetStatPointsChar4gold = GameManager.Instance.PartyMembers[3].baseStats.level * 500 + 2000;
        goldCount = GameManager.Instance.GetGold();
        goldText.text = $"Gold: {goldCount}";
        resetStatPointsChar1goldText.text = $"Price: {GameManager.Instance.PartyMembers[0].baseStats.level * 500 + 2000}";
        resetStatPointsChar2goldText.text = $"Price: {GameManager.Instance.PartyMembers[1].baseStats.level * 500 + 2000}";
        resetStatPointsChar3goldText.text = $"Price: {GameManager.Instance.PartyMembers[2].baseStats.level * 500 + 2000}";
        resetStatPointsChar4goldText.text = $"Price: {GameManager.Instance.PartyMembers[3].baseStats.level * 500 + 2000}";
        upgradeDyngeonPassgoldText.text = $"Price: {1000+ GameManager.Instance.GetPassLevel() *400}";


        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        resetStatPointsChar1.onClick.RemoveAllListeners();
        resetStatPointsChar1.onClick.AddListener(() =>
        {
            if(resetStatPointsChar1gold <= goldCount)
            {
                resetKnightStats();
                GameManager.Instance.RemoveGold(resetStatPointsChar1gold);
                updateGold();
            }
        });

        resetStatPointsChar2.onClick.RemoveAllListeners();
        resetStatPointsChar2.onClick.AddListener(() =>
        {
            if (resetStatPointsChar2gold <= goldCount)
            {
                resetGuardianStats();
                GameManager.Instance.RemoveGold(resetStatPointsChar2gold);
                updateGold();
            }
        });
        resetStatPointsChar3.onClick.RemoveAllListeners();
        resetStatPointsChar3.onClick.AddListener(() =>
        {
            if (resetStatPointsChar3gold <= goldCount)
            {
                resetPaladinStats();
                GameManager.Instance.RemoveGold(resetStatPointsChar3gold);
                updateGold();
            }
        });
        resetStatPointsChar4.onClick.RemoveAllListeners();
        resetStatPointsChar4.onClick.AddListener(() =>
        {
            if (resetStatPointsChar1gold <= goldCount)
            {
                resetWizardStats();
                GameManager.Instance.RemoveGold(resetStatPointsChar4gold);
                updateGold();
            }
        });

        upgradeDyngeonPass.onClick.RemoveAllListeners();
        upgradeDyngeonPass.onClick.AddListener(() =>
        {
            if (1000 + GameManager.Instance.GetPassLevel() * 400 <= goldCount)
            {
                GameManager.Instance.RemoveGold(1000 + GameManager.Instance.GetPassLevel() * 400);
                GameManager.Instance.increasePassLevel();
                upgradeDyngeonPassgoldText.text = $"Price: {1000 + GameManager.Instance.GetPassLevel() * 400}";
                updateGold();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void updateGold()
    {
        goldText.text = $"Gold: {GameManager.Instance.GetGold()}";
    }
    void resetKnightStats()
    {
        Characters currentBaseStats = GameManager.Instance.PartyMembers[0].baseStats;

        currentBaseStats.avaliableStatPoints = 0;
        currentBaseStats.maxHealth = 120 + currentBaseStats.level *10;
        currentBaseStats.attack = 15 + currentBaseStats.level;
        currentBaseStats.defense = 10 + currentBaseStats.level;
        currentBaseStats.intelligence = 5 + currentBaseStats.level;
        currentBaseStats.magicDefence = 6 + currentBaseStats.level;
        currentBaseStats.Luck = 10 + currentBaseStats.level;

        if (currentBaseStats.level <= 10)
        {
            currentBaseStats.avaliableStatPoints += 3 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 20)
        {
            currentBaseStats.avaliableStatPoints += 30;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 40)
        {
            currentBaseStats.avaliableStatPoints += 50;
            currentBaseStats.avaliableStatPoints += 4 * currentBaseStats.level;
        }
        else
        {
            currentBaseStats.avaliableStatPoints += 130;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
    }
    void resetGuardianStats()
    {
        Characters currentBaseStats = GameManager.Instance.PartyMembers[1].baseStats;

        currentBaseStats.avaliableStatPoints = 0;
        currentBaseStats.maxHealth = 150 + currentBaseStats.level * 10;
        currentBaseStats.attack = 10 + currentBaseStats.level;
        currentBaseStats.defense = 15 + currentBaseStats.level;
        currentBaseStats.intelligence = 4 + currentBaseStats.level;
        currentBaseStats.magicDefence = 8 + currentBaseStats.level;
        currentBaseStats.Luck = 10 + currentBaseStats.level;

        if (currentBaseStats.level <= 10)
        {
            currentBaseStats.avaliableStatPoints += 3 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 20)
        {
            currentBaseStats.avaliableStatPoints += 30;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 40)
        {
            currentBaseStats.avaliableStatPoints += 50;
            currentBaseStats.avaliableStatPoints += 4 * currentBaseStats.level;
        }
        else
        {
            currentBaseStats.avaliableStatPoints += 130;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
    }
    void resetPaladinStats()
    {
        Characters currentBaseStats = GameManager.Instance.PartyMembers[2].baseStats;

        currentBaseStats.avaliableStatPoints = 0;
        currentBaseStats.maxHealth = 135 + currentBaseStats.level * 10;
        currentBaseStats.attack = 12 + currentBaseStats.level;
        currentBaseStats.defense = 12 + currentBaseStats.level;
        currentBaseStats.intelligence = 10 + currentBaseStats.level;
        currentBaseStats.magicDefence = 12 + currentBaseStats.level;
        currentBaseStats.Luck = 10 + currentBaseStats.level;

        if (currentBaseStats.level <= 10)
        {
            currentBaseStats.avaliableStatPoints += 3 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 20)
        {
            currentBaseStats.avaliableStatPoints += 30;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 40)
        {
            currentBaseStats.avaliableStatPoints += 50;
            currentBaseStats.avaliableStatPoints += 4 * currentBaseStats.level;
        }
        else
        {
            currentBaseStats.avaliableStatPoints += 130;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
    }
    void resetWizardStats()
    {
        Characters currentBaseStats = GameManager.Instance.PartyMembers[1].baseStats;

        currentBaseStats.avaliableStatPoints = 0;
        currentBaseStats.maxHealth = 80 + currentBaseStats.level * 10;
        currentBaseStats.attack = 5 + currentBaseStats.level;
        currentBaseStats.defense = 5 + currentBaseStats.level;
        currentBaseStats.intelligence = 18 + currentBaseStats.level;
        currentBaseStats.magicDefence = 10 + currentBaseStats.level;
        currentBaseStats.Luck = 10 + currentBaseStats.level;

        if (currentBaseStats.level <= 10)
        {
            currentBaseStats.avaliableStatPoints += 3 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 20)
        {
            currentBaseStats.avaliableStatPoints += 30;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
        else if (currentBaseStats.level <= 40)
        {
            currentBaseStats.avaliableStatPoints += 50;
            currentBaseStats.avaliableStatPoints += 4 * currentBaseStats.level;
        }
        else
        {
            currentBaseStats.avaliableStatPoints += 130;
            currentBaseStats.avaliableStatPoints += 2 * currentBaseStats.level;
        }
    }
    void calculateGoldPrice(PartyMember member)
    {

    }
    int getGoldForPass()
    {
        return 0;
    }
}
