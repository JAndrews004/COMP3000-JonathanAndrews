using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatEndScreen : MonoBehaviour
{
    public GameObject screen;
    public TextMeshProUGUI endMessage;
    public TextMeshProUGUI character1Name;
    public TextMeshProUGUI character2Name;
    public TextMeshProUGUI character3Name;
    public TextMeshProUGUI character4Name;

    public Image character1Image;
    public Image character2Image;
    public Image character3Image;
    public Image character4Image;


    public Slider character1XpBar;
    public Slider character2XpBar;
    public Slider character3XpBar;
    public Slider character4XpBar;

    public TextMeshProUGUI character1LevelUpText;
    public TextMeshProUGUI character2LevelUpText;
    public TextMeshProUGUI character3LevelUpText;
    public TextMeshProUGUI character4LevelUpText;

    public Button ContinueButton;
    public Button ExitButton;

    private float AnimationSpeed;
    public void Bind(List<PartyMember> members, bool Win)
    {
        screen.SetActive(true);
        if (Win)
        {
            endMessage.text = "Congratulations!";
        }
        else
        {
            endMessage.text = "You lost";
            ContinueButton.interactable = false;



        }
        if(members.Count >= 4)
        {
            
            character1Name.text = $"{members[0].baseStats.characterName} - level {members[0].baseStats.level}";
            character1Image.sprite = members[0].baseStats.HeadShot;
            
            character2Name.text = $"{members[1].baseStats.characterName} - level {members[1].baseStats.level}";
            character2Image.sprite = members[1].baseStats.HeadShot;
            
            character3Name.text = $"{members[2].baseStats.characterName} - level {members[2].baseStats.level}";
            character3Image.sprite = members[2].baseStats.HeadShot;
            
            character4Name.text = $"{members[3].baseStats.characterName} - level {members[3].baseStats.level}";
            character4Image.sprite = members[3].baseStats.HeadShot;
            


            character1XpBar.maxValue = members[0].XpToLevelUp;
            character1XpBar.value = members[0].Xp;

            character2XpBar.maxValue = members[1].XpToLevelUp;
            character2XpBar.value = members[1].Xp;

            character3XpBar.maxValue = members[2].XpToLevelUp;
            character3XpBar.value = members[2].Xp;

            character4XpBar.maxValue = members[3].XpToLevelUp;
            character4XpBar.value = members[3].Xp;

        }


        ContinueButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.AddListener(() =>
        {
            GameManager.Instance.EndCombat();
        });

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.EndCombat();
        });
    }

    public IEnumerator UpdateXPChar1(PartyMember mem, int xpAdded)
    {
        float currentVal = character1XpBar.value;
        float targetVal = mem.Xp;
        AnimationSpeed = xpAdded;
        Debug.Log($"{xpAdded} added to {currentVal} to get to {targetVal}");
        if (currentVal + xpAdded >= character1XpBar.maxValue)
        {
            Debug.Log($"Animating to max");
            yield return AnimateSlider(currentVal, character1XpBar.maxValue, character1XpBar);

            character1XpBar.value = 0;
            character1XpBar.maxValue = mem.XpToLevelUp;
            character1LevelUpText.enabled = true;

            Debug.Log($"Updated slider values now Animating to targetVal");
            yield return AnimateSlider(0, targetVal, character1XpBar);
        }
        else
        {
            Debug.Log($"Animating to targetVal");
            yield return AnimateSlider(currentVal, targetVal, character1XpBar);
        }
        


        yield return new WaitForSeconds(0.5f);

    }

    public IEnumerator UpdateXPChar2(PartyMember mem, int xpAdded)
    {
        float currentVal = character2XpBar.value;
        float targetVal = mem.Xp;
        AnimationSpeed = xpAdded / 100;
        if (currentVal + xpAdded >= character2XpBar.maxValue)
        {
            yield return AnimateSlider(currentVal, character2XpBar.maxValue, character2XpBar);

            character2XpBar.value = 0;
            character2XpBar.maxValue = mem.XpToLevelUp;
            character2LevelUpText.enabled = true;

            yield return AnimateSlider(0, targetVal, character2XpBar);
        }
        else if (targetVal >= currentVal)
        {
            yield return AnimateSlider(currentVal, targetVal, character2XpBar);
        }



        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator UpdateXPChar3(PartyMember mem, int xpAdded)
    {
        float currentVal = character3XpBar.value;
        float targetVal = mem.Xp;
        AnimationSpeed = xpAdded / 100;
        if (currentVal + xpAdded >= character3XpBar.maxValue)
        {
            yield return AnimateSlider(currentVal, character3XpBar.maxValue, character3XpBar);

            character3XpBar.value = 0;
            character3XpBar.maxValue = mem.XpToLevelUp;
            character3LevelUpText.enabled = true;

            yield return AnimateSlider(0, targetVal, character3XpBar);
        }
        else if (targetVal >= currentVal)
        {
            yield return AnimateSlider(currentVal, targetVal, character3XpBar);
        }



        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator UpdateXPChar4(PartyMember mem, int xpAdded)
    {
        float currentVal = character4XpBar.value;
        float targetVal = mem.Xp;
        AnimationSpeed = xpAdded / 100;
        if (currentVal + xpAdded >= character4XpBar.maxValue)
        {
            yield return AnimateSlider(currentVal, character4XpBar.maxValue, character4XpBar);

            character4XpBar.value = 0;
            character4XpBar.maxValue = mem.XpToLevelUp;
            character4LevelUpText.enabled = true;

            yield return AnimateSlider(0, targetVal, character4XpBar);
        }
        else if (targetVal >= currentVal)
        {
            yield return AnimateSlider(currentVal, targetVal, character4XpBar);
        }



        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator AnimateSlider(float startVal, float endVal,Slider slider)
    {
        if(AnimationSpeed <= 0)
        {
            Debug.Log($"Animation speed 0");
            slider.value = endVal;
            yield break;
        }
        float t = startVal;

        while (t < endVal)
        {
            t += AnimationSpeed * Time.deltaTime;
            
            slider.value = Mathf.Clamp(t,startVal,endVal);
            yield return null;
        }

        
    }
}
