using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonBoardEntry : MonoBehaviour
{
    public DungeonData data;
    public TextMeshProUGUI name;
    public TextMeshProUGUI tier;
    public TextMeshProUGUI length;
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI recommendedLevel;
    public TextMeshProUGUI xpScaling;
    public TextMeshProUGUI goldScaling;
    public Button Select;
    public Image mainElement;
    public Image secondaryElement;

    public List<Sprite> sprites;
    bool locked;

    public void Bind(DungeonData dungeon)
    {
        data = dungeon;
        name.text = data.name;
        locked = true;
        switch (data.requiredPass)
        {
            case CharacterClass.F:
                tier.text = $"Tier F pass required";
                if (GameManager.Instance.GetPassLevel() >=0)
                {
                    locked = false;
                }
                break;
            case CharacterClass.E:
                tier.text = $"Tier E pass required";
                if (GameManager.Instance.GetPassLevel() >= 1)
                {
                    locked = false;
                }
                break;
            case CharacterClass.D:
                tier.text = $"Tier D pass required";
                if (GameManager.Instance.GetPassLevel() >= 2)
                {
                    locked = false;
                }
                break;
            case CharacterClass.C:
                tier.text = $"Tier C pass required";
                if (GameManager.Instance.GetPassLevel() >= 3)
                {
                    locked = false;
                }
                break;
            case CharacterClass.B:
                tier.text = $"Tier B pass required";
                if (GameManager.Instance.GetPassLevel() >= 4)
                {
                    locked = false;
                }
                break;
            case CharacterClass.A:
                tier.text = $"Tier A pass required";
                if (GameManager.Instance.GetPassLevel() >= 5)
                {
                    locked = false;
                }
                break;
            case CharacterClass.S:
                tier.text = $"Tier S pass required";
                if (GameManager.Instance.GetPassLevel() == 6)
                {
                    locked = false;
                }
                break;
        }
        tier.color = locked ? Color.red : Color.green;
        switch (data.length)
        {
            case Length.Short:
                length.text = "Short";
                break;
            case Length.Medium:
                length.text = "Medium";
                break;
            case Length.Long:
                length.text = "Long";
                break;
        }
        switch (data.difficulty)
        {
            case 1:
                difficulty.text = "Easy";
                break;
            case 2:
                difficulty.text = "Medium";
                break;
            case 3:
                difficulty.text = "Hard";
                break;
            case 4:
                difficulty.text = "Insane";
                break;
        }
        recommendedLevel.text = $"Recommended Level: {data.recommendedLevel}";

        xpScaling.text = $"XP {new string('+', data.expectedRewardXP)}";
        goldScaling.text = $"Gold {new string('+', data.expectedRewardGold)}";

        Select.onClick.RemoveAllListeners();
        Select.onClick.AddListener(() => {
            selectedDungeon();
            if (GameManager.Instance.tutorialActive)
            {
                GameManager.Instance.tutorialManager.StartDungeonTut();
            }
        });

        // set element icons

        switch (data.mainElement)
        {
            case Element.None:
                mainElement.sprite = sprites[0];
                break;
            case Element.Fire:
                mainElement.sprite = sprites[1];
                break;
            case Element.Earth:
                mainElement.sprite = sprites[2];
                break;
            case Element.Air:
                mainElement.sprite = sprites[3];
                break;
            case Element.Water:
                mainElement.sprite = sprites[3];
                break;
        }

        switch (data.secondaryElement)
        {
            case Element.None:
                secondaryElement.sprite = sprites[0];
                break;
            case Element.Fire:
                secondaryElement.sprite = sprites[1];
                break;
            case Element.Earth:
                secondaryElement.sprite = sprites[2];
                break;
            case Element.Air:
                secondaryElement.sprite = sprites[3];
                break;
            case Element.Water:
                secondaryElement.sprite = sprites[3];
                break;
        }

    }
    public void selectedDungeon()
    {
        if (!locked)
        {
            GameManager.Instance.selectedDungeon = data;
        }
        //check if player can select
        // set selectedDungeon in GameManager
    }
}
public enum Length
{
    Short,
    Medium,
    Long,
}
