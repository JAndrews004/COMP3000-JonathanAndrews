using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveManager : MonoBehaviour
{
    public GameStats gameStats;
    public List<Characters> characters;
    public List<AbilityData> abilities;

#if UNITY_EDITOR
    [ContextMenu("Auto Fill Abilities")]
    private void AutoFillAbilities()
    {
        abilities = new List<AbilityData>();

        string[] guids = AssetDatabase.FindAssets("t:AbilityData");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AbilityData ability = AssetDatabase.LoadAssetAtPath<AbilityData>(path);

            abilities.Add(ability);
        }

        Debug.Log("Abilities loaded: " + abilities.Count);
    }
#endif

    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
        LoadGame();
    }


    public void SaveGame()
    {
        savePath = Application.persistentDataPath + "/save.json";
        SaveData data = new SaveData();

        data.money = gameStats.Gold;
        data.tutorialCompleted = gameStats.tutorial;
        data.passLevel = gameStats.passLevel;

        foreach (var character in characters)
        {
            CharacterSaveData cData = new CharacterSaveData();

            cData.characterID = character.characterID;

            cData.level = character.level;
            cData.xp = character.xp;

            cData.maxHealth = character.maxHealth;
            cData.attack = character.attack;
            cData.defense = character.defense;
            cData.intelligence = character.intelligence;
            cData.magicDefence = character.magicDefence;
            cData.luck = character.Luck;
            cData.availableStatPoints = character.avaliableStatPoints;

            foreach (var ability in character.equippedAbilities)
            {
                if(ability != null) 
                    cData.equippedAbilityIDs.Add(ability.abilityID);
            }

            if (character.EquippedElement != null)
            {
                cData.equippedElementID = character.EquippedElement.abilityID;
            }

            data.characters.Add(cData);
        }

        foreach (var ability in abilities)
        {
            if (ability.unlocked)
            {
                data.unlockedAbilityIDs.Add(ability.abilityID);
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved to: " + savePath);
    }

    public void LoadGame()
    {
        savePath = Application.persistentDataPath + "/save.json";
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        gameStats.Gold = data.money;
        gameStats.tutorial = data.tutorialCompleted;
        gameStats.passLevel = data.passLevel;

        // Reset all abilities first
        foreach (var ability in abilities)
        {
            ability.unlocked = false;
        }

        foreach (string id in data.unlockedAbilityIDs)
        {
            AbilityData ability = abilities.Find(a => a.abilityID == id);

            if (ability != null)
            {
                ability.unlocked = true;
            }
        }

 
        foreach (var cData in data.characters)
        {
            Characters character = characters.Find(c => c.characterID == cData.characterID);

            if (character == null) continue;

            character.level = cData.level;
            character.xp = cData.xp;

            character.maxHealth = cData.maxHealth;
            character.attack = cData.attack;
            character.defense = cData.defense;
            character.intelligence = cData.intelligence;
            character.magicDefence = cData.magicDefence;
            character.Luck = cData.luck;
            character.avaliableStatPoints = cData.availableStatPoints;

            character.equippedAbilities.Clear();

            for (int i = 0; i < 6; i++)
            {
                if (i < cData.equippedAbilityIDs.Count)
                {
                    string id = cData.equippedAbilityIDs[i];

                    AbilityData ability = abilities.Find(a => a.abilityID == id);

                    if (ability != null)
                    {
                        character.equippedAbilities.Add(ability);
                    }
                    else
                    {
                        character.equippedAbilities.Add(null);
                    }
                }
                else
                {
                    character.equippedAbilities.Add(null);
                }
            }

            character.EquippedElement = abilities.Find(a => a.abilityID == cData.equippedElementID);
        }

        Debug.Log("Game Loaded");
    }
}