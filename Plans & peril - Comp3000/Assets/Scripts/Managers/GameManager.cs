using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers;

    public List<GameObject> EasyEnemyPrefabs;
    public GameStats Stats;

    private int gold;
    private int passLevel;

    public bool InCombat = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else 
        { 
            Instance = this;
            gold = Stats.Gold;
            passLevel = Stats.passLevel;
            DontDestroyOnLoad(gameObject);
        }
 
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    void StartGame()
    {

    }
    void EndGame()
    {

    }

    void PauseGame()
    {

    }
    void ResumeGame()
    {

    }

    void ResetGame()
    {

    }
    public void AddGold(int points)
    {
        gold += points;
        Stats.Gold = gold;
    }

    public void RemoveGold(int points) 
    {
        gold -= points;
        if(gold< 0)
        {
            gold = 0;
        }
        Stats.Gold = gold;
    }
    public int GetGold()
    {
        return gold;
    }

    public int GetPassLevel()
    {
        return passLevel;
    }
    public void increasePassLevel()
    {
        passLevel++;
    }
    void LoadLevel(int levelIndex)
    {

    }
    void ReloadLevel()
    {

    }
    void LoadNextLevel()
    {

    }

    public void StartCombat()
    {
        for(int i =0; i< 6; i++)
        {
            GameObject newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
            newEnemy.transform.parent = transform;
        }
        RefreshEnemyMembers();
        SceneManager.LoadScene(1);
        populateAbilities();
        InCombat = true;
        
    }

    public void EndCombat()
    {
        SceneManager.LoadScene(0);
        InCombat = false;
    }

    public void RefreshPartyMembers()
    {
        PartyMembers = new List<PartyMember>(GetComponentsInChildren<PartyMember>());
    }

    public void RefreshEnemyMembers()
    {
        EnemyMembers = new List<EnemyMember>(GetComponentsInChildren<EnemyMember>());
    }

    public void populateAbilities()
    {
        RefreshPartyMembers();
        foreach (CombatMember member in PartyMembers)
        {
            foreach (AbilityData data in member.abilityDatas)
            {
                if (data != null)
                {
                    if(data.abilityCategory == AbilityData.AbilityCategory.Active)
                    {
                        Ability ability = new Ability(data);
                        Debug.Log(ability.AbilityData.abilityName + " added to active ability list");
                        member.activeAbilities.Add(ability);
                    }
                    else
                    {
                        Ability ability = new Ability(data);
                        Debug.Log(ability.AbilityData.abilityName + " added to passive ability list");
                        member.passiveAbilities.Add(ability);
                    }

                }

            }
        }

        RefreshEnemyMembers();
        foreach (CombatMember member in EnemyMembers)
        {
            if (member == null)
            {
                Debug.LogError("Null EnemyMember found in EnemyMembers list!");
                continue;
            }

            if (member.activeAbilities == null)
                member.activeAbilities = new List<Ability>();

            if (member.abilityDatas == null)
                member.abilityDatas = new List<AbilityData>();

            foreach (AbilityData data in member.abilityDatas)
            {
                if (data == null) continue;

                if (data.abilityCategory == AbilityData.AbilityCategory.Active)
                {
                    Ability ability = new Ability(data);
                    Debug.Log(ability.AbilityData.abilityName + " added to active ability list");
                    member.activeAbilities.Add(ability);
                }
                else
                {
                    Ability ability = new Ability(data);
                    Debug.Log(ability.AbilityData.abilityName + " added to passive ability list");
                    member.passiveAbilities.Add(ability);
                }
            }
        }

    }
    }
