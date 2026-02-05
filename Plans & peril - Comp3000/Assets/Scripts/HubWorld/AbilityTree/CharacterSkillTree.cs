using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillTree : MonoBehaviour
{
    public PartyMember pm;
    public List<AbilityData> unlockedAbilities;
    
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PartyMember>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUnlocked(AbilityData ability)
    {
        foreach (AbilityData ad  in unlockedAbilities)
        {
            if(ability == ad)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CanUnlock(AbilityData ability)
    {
        foreach (AbilityData required in ability.prerequisiteAbilities)
        {
            bool flag = false;
            foreach(AbilityData ad in unlockedAbilities)
            {
                if (required == ad)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return false;
            }
 
        }
        if (ability.goldCost > GameManager.Instance.GetGold())
        {
            return false;
        }
        else if(ability.strengthRequired > pm.CurrentAttack)
        {
            return false;
        }
        else if (ability.defenseRequired > pm.CurrentDefense)
        {
            return false;
        }
        else if (ability.intelligenceRequired > pm.CurrentIntelligence)
        {
            return false;
        }
        else if (ability.magicDefenseRequired > pm.CurrentMagicDefense)
        {
            return false;
        }
        else if (ability.luckRequired > pm.CurrentLuck)
        {
            return false;
        }
        else if (ability.vitalityRequired > pm.CurrentMaxHealth /10)
        {
            return false;
        }
        
        return true;
    }
    public void UnlockAbility(AbilityData ability)
    {
        if (CanUnlock(ability))
        {
            ability.unlocked = true;
            unlockedAbilities.Add(ability);
            GameManager.Instance.RemoveGold(ability.goldCost);

        }
    }
}
