using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public List<PartyMember> PartyMembers;

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
    void AddGold(int points)
    {

    }

    void RemoveGold(int points) 
    { 
    
    }

    void AddXP(int xp)
    {

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
        SceneManager.LoadScene(1);
        InCombat = true;
    }

    void EndCombat()
    {

    }

    public void RefreshPartyMembers()
    {
        PartyMembers = new List<PartyMember>(GetComponentsInChildren<PartyMember>());
    }

}
