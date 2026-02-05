

using JetBrains.Annotations;
using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public FXManager fXManager;
    public List<PartyMember> PartyMembers;
    public List<EnemyMember> EnemyMembers;

    public List<GameObject> EasyEnemyPrefabs = new List<GameObject>() { };
    public GameStats Stats;

    private int gold;
    private int passLevel;

    public List<GameObject> enemyObjects = new List<GameObject>();
    public DungeonData selectedDungeon;
    public DungeonRuntimeState dungeonRuntimeState;
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
            DontDestroyOnLoad(gameObject);
            if (Stats == null)
            {
                return;
            }
            gold = Stats.Gold;
            passLevel = Stats.passLevel;
            
        }
 
    }

    public static void SetInstanceForTesting(GameManager gm)
    {
        Instance = gm;
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

    public void StartCombat()
    {
        PrepareCombatData();
        LoadCombatScene();
    }
    public void PrepareCombatData()
    {
        if (EasyEnemyPrefabs != null && EasyEnemyPrefabs.Count > 0 && selectedDungeon!=null && dungeonRuntimeState!=null)
        {
            int numOfEnemies = Random.RandomRange(1, 6);
            int chance = Random.RandomRange(0, 100);
            EnemyMember.Tier enemyTiers = EnemyMember.Tier.Easy;
            if(dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom].roomType == RoomType.Elite)
            {
                chance -= 20;
            }
            switch (selectedDungeon.difficulty)
            {
                case 1:
                    enemyTiers = EnemyMember.Tier.Easy;
                    if (chance <= 40)
                    {
                        numOfEnemies = Random.RandomRange(2, 5);
                    }
                    else
                    {
                        numOfEnemies = Random.RandomRange(1, 3);
                    }
                    break;
                case 2:
                    enemyTiers = EnemyMember.Tier.Medium;
                    if (chance <= 45)
                    {
                        numOfEnemies = Random.RandomRange(2, 5);
                    }
                    else
                    {
                        numOfEnemies = Random.RandomRange(1, 4);
                    }
                    break;
                case 3:
                    enemyTiers = EnemyMember.Tier.Hard;
                    if (chance <= 50)
                    {
                        numOfEnemies = Random.RandomRange(4, 6);
                    }
                    else
                    {
                        numOfEnemies = Random.RandomRange(1, 5);
                    }
                    break;
            }
            for (int i = 0; i < numOfEnemies; i++)
            {
                chance = Random.RandomRange(0, 100);
                
                if(dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom].roomType == RoomType.Normal || dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom].roomType == RoomType.Elite)
                {
                    GameObject newEnemy;
                    switch (enemyTiers)
                    {
                        case EnemyMember.Tier.Easy:
                            newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
                            newEnemy.transform.parent = transform;
                            enemyObjects.Add(newEnemy);
                            break;
                        case EnemyMember.Tier.Medium:
                            newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);//change to medium enemy prefabs
                            newEnemy.transform.parent = transform;
                            enemyObjects.Add(newEnemy);
                            break;
                        case EnemyMember.Tier.Hard:
                            newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
                            newEnemy.transform.parent = transform;
                            enemyObjects.Add(newEnemy);
                            break;
                    }
                    
                }
                else if(dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom].roomType == RoomType.Boss)
                { 
                    bool bossCreated = false;
                    GameObject newEnemy;
                    if (!bossCreated)
                    {
                        newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]); // change to boss enemy prefabs
                        newEnemy.transform.parent = transform;
                        enemyObjects.Add(newEnemy);
                    }
                    else { 
                        switch (enemyTiers)
                        {
                            case EnemyMember.Tier.Easy:
                                newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
                                newEnemy.transform.parent = transform;
                                enemyObjects.Add(newEnemy);
                                break;
                            case EnemyMember.Tier.Medium:
                                newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);//change to medium enemy prefabs
                                newEnemy.transform.parent = transform;
                                enemyObjects.Add(newEnemy);
                                break;
                            case EnemyMember.Tier.Hard:
                                newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
                                newEnemy.transform.parent = transform;
                                enemyObjects.Add(newEnemy);
                                break;
                        }
                    }
                   
                }     
      
            }
        }
        else if(EasyEnemyPrefabs != null && EasyEnemyPrefabs.Count > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject newEnemy = Instantiate(EasyEnemyPrefabs[Random.Range(0, EasyEnemyPrefabs.Count)]);
                newEnemy.transform.parent = transform;
            }
        }
        else
        {
            Debug.LogWarning("No EasyEnemyPrefabs assigned!");
        }
        RefreshEnemyMembers();

        populateAbilities();

        InCombat = true;
    }
    public void LoadCombatScene()
    {
        dungeonRuntimeState.destroyRooms();
        SceneManager.LoadScene(1);
    }
    public void EndCombat()
    {
        if(dungeonRuntimeState != null && dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom] != null && dungeonRuntimeState.rooms[dungeonRuntimeState.currentRoom].roomType != RoomType.Boss)
        {
            foreach (GameObject obj in enemyObjects)
            {
                Destroy(obj);
            }
            LoadDungeonScene();
        }
        else
        {
            loadHubWorld();
            foreach (CombatMember member in PartyMembers)
            {
                member.activeAbilities.Clear();
                member.passiveAbilities.Clear();
            }
        }

        InCombat = false;
    }
    public void loadHubWorld()
    {
        dungeonRuntimeState.destroyRooms();
        SceneManager.LoadScene(0);
        dungeonRuntimeState.ResetDungeonLayout();
        foreach (GameObject obj in enemyObjects)
        {
            Destroy(obj);
        }
        InCombat = false;
    }

    public void RefreshPartyMembers()
    {
        PartyMembers = new List<PartyMember>(GetComponentsInChildren<PartyMember>());
    }

    public void RefreshEnemyMembers()
    {
        EnemyMembers = new List<EnemyMember>(GetComponentsInChildren<EnemyMember>());
        int averagePlayerLevel = 0;
        foreach (PartyMember mem in PartyMembers)
        {
            averagePlayerLevel += mem.baseStats.level;
        }
        averagePlayerLevel = averagePlayerLevel / PartyMembers.Count;
        int EnemyLevels = selectedDungeon.recommendedLevel;

        foreach(EnemyMember enemy in EnemyMembers)
        {
            EnemyLevels = selectedDungeon.recommendedLevel + Random.RandomRange(-Mathf.RoundToInt(EnemyLevels *0.2f),Mathf.RoundToInt(EnemyLevels * 0.2f));
            if(EnemyLevels <= 0)
            {
                EnemyLevels = 0;
            }
            int levelDiff = EnemyLevels - averagePlayerLevel;
            float multiplier = 1 + (levelDiff * 0.2f);
            int xp = Mathf.RoundToInt(Mathf.Clamp(multiplier, 0.1f, 2.0f) * 10 * (2 * EnemyLevels + 1));
            switch (enemy.tier)
            {
                case EnemyMember.Tier.Easy:
                    enemy.Init_Enemy(EnemyLevels, Mathf.RoundToInt(xp * 0.8f));
                    break;
                case EnemyMember.Tier.Medium:
                    enemy.Init_Enemy(EnemyLevels, xp);
                    break;
                case EnemyMember.Tier.Hard:
                    enemy.Init_Enemy(EnemyLevels, Mathf.RoundToInt(xp * 1.2f));
                    break;
                case EnemyMember.Tier.Boss:
                    enemy.Init_Enemy(EnemyLevels, Mathf.RoundToInt(xp * 1.5f));
                    break;
            }
        }
    }

    public void populateAbilities()
    {
        RefreshPartyMembers();
        foreach (CombatMember member in PartyMembers)
        {
            if(member.passiveAbilities.Count <=0 && member.activeAbilities.Count <= 0)
            {
                foreach (AbilityData data in member.abilityDatas)
                {
                    if (data != null)
                    {
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
    public void LoadDungeonScene()
    {
        dungeonRuntimeState.currentData = selectedDungeon;
        if (dungeonRuntimeState.rooms.Count <= 0)
        {
            dungeonRuntimeState.GenerateRooms();
        }
        SceneManager.LoadScene(3);
        dungeonRuntimeState.DrawRooms();
    }
}
