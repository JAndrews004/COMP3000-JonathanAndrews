using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUnlockManager : MonoBehaviour
{
    public TextMeshProUGUI AbilityName;
    
    
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI DefenseStat;
    public TextMeshProUGUI IntelligenceStat;
    public TextMeshProUGUI MagicDefenseStat;
    public TextMeshProUGUI LuckStat;
    public TextMeshProUGUI HpStat;

    public TextMeshProUGUI Description;
    public TextMeshProUGUI Price;

    public Button buyButton;

    public Image AbilityIcon;
    public Image elementIcon;
    public Sprite neutralIcon;
    public Sprite earthIcon;
    public Sprite fireIcon;
    public Sprite waterIcon;
    public Sprite airIcon;

    public SkillTreeUIContoller SkillTree;
    public GameObject InfoPannel;
    public void Start()
    {
        if (GetComponentInParent<SkillsMenu>().chosenCharacter.characterSkillTree.unlockedAbilities != null)
        {
            setAbilityUI(GetComponentInParent<SkillsMenu>().chosenCharacter.characterSkillTree.unlockedAbilities[0]);
        }
        
    }
    public void setAbilityUI(AbilityData abilityData)
    {
        Debug.Log("Setting texts");
        AbilityName.text = abilityData.abilityName;
        PartyMember member = GetComponentInParent<SkillsMenu>().chosenCharacter;
        AbilityIcon.sprite = abilityData.icon;
        SetStat(DamageStat, "STR", abilityData.strengthRequired, member.CurrentAttack);
        SetStat(DefenseStat, "DEF", abilityData.defenseRequired, member.CurrentDefense);
        SetStat(IntelligenceStat, "INT", abilityData.intelligenceRequired, member.CurrentIntelligence);
        SetStat(MagicDefenseStat, "MDF", abilityData.magicDefenseRequired, member.CurrentMagicDefense);
        SetStat(LuckStat, "LCK", abilityData.luckRequired, member.CurrentLuck);
        SetStat(HpStat, "VIT", abilityData.vitalityRequired, member.CurrentMaxHealth/10);

        switch (abilityData.elementTag)
        {
            case Element.None:
                elementIcon.sprite = neutralIcon;
                break;
            case Element.Air:
                elementIcon.sprite = airIcon;
                break;
            case Element.Fire:
                elementIcon.sprite = fireIcon;
                break;
            case Element.Water:
                elementIcon.sprite = waterIcon;
                break;
            case Element.Earth:
                elementIcon.sprite = earthIcon;
                break;
        }


        int price = 0;
        foreach(AbilityData data in member.characterSkillTree.unlockedAbilities)
        {
            price += 1;
        }

        price *= 1000;

        if(price > GameManager.Instance.GetGold()|| member.characterSkillTree.IsUnlocked(abilityData)|| !member.characterSkillTree.CanUnlock(abilityData))
        {
            buyButton.interactable = false;
            SetStat(Price, "Price: ", price, GameManager.Instance.GetGold(),"g");
        }
        else
        {
            buyButton.interactable = true;
            SetStat(Price, "Price: ", price, GameManager.Instance.GetGold(), "g");
        }
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            if (member.characterSkillTree.CanUnlock(abilityData))
            {
                member.characterSkillTree.UnlockAbility(abilityData);
                setAbilityUI(abilityData);
                SkillTree.updateSkillTree();
            }

        });
        Debug.Log("Finished");
        Description.text = abilityData.description;
    }
    void SetStat(TextMeshProUGUI statText, string label, int required, int current)
    {
        statText.text = $"{label}:{required}";
        statText.color = required <= current ? Color.green : Color.red;
    }
    void SetStat(TextMeshProUGUI statText, string label, int required, int current,string end)
    {
        statText.text = $"{label}:{required}{end}";
        statText.color = required <= current ? Color.green : Color.red;
    }

    public void OpenInfoPannel()
    {
        InfoPannel.SetActive(true);
    }
    public void CloseInfoPannel()
    {
        InfoPannel.SetActive(false);
    }
}
