using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubWorldManager : MonoBehaviour
{

    public Button SkillsMenu;
    public Button Shop;
    public Button Training;
    public Button DungeonSelection;

    public GameObject SkillsPanel;
    public GameObject ShopPanel;
    public GameObject TrainingPanel;
    public GameObject DungeonPanel;


    void Start()
    {
        SkillsMenu.onClick.RemoveAllListeners();
        SkillsMenu.onClick.AddListener(() =>
        {
            SkillsPanel.SetActive(true);
        });
        Shop.onClick.RemoveAllListeners();
        Shop.onClick.AddListener(() =>
        {
            ShopPanel.SetActive(true);
        });
        Training.onClick.RemoveAllListeners();
        Training.onClick.AddListener(() =>
        {
            TrainingPanel.SetActive(true);
        });
        DungeonSelection.onClick.RemoveAllListeners();
        DungeonSelection.onClick.AddListener(() =>
        {
            DungeonPanel.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartDungeon()
    {
        foreach (var character in GameManager.Instance.PartyMembers)
        {
            character.CurrentHealth = character.CurrentMaxHealth;
        }
        GameManager.Instance.StartCombat();
    }
}
