using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public AbilityData AbilityData;

    public int cooldownLeft;
    public int usesLeft;

    public Ability(AbilityData abilityData)
    {
        if(abilityData != null)
        {
            AbilityData = abilityData;
            usesLeft = abilityData.maxUsage; // Reset when reloaded or instantiated
            cooldownLeft = 0;
        }
        
    }
    public void DecreaseUses()
    {
        usesLeft--;

        if (usesLeft <= 0)
        {
            Debug.Log("Uses depleated for: " + AbilityData.abilityName);
            usesLeft = 0;
        }
    }

    public void DecreaseCooldown()
    {
        cooldownLeft--;
        if (cooldownLeft <= 0)
        {
            cooldownLeft = 0;
        }
    }

}
