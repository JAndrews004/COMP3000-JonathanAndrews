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

    public Button StartDungeon;

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

        GameManager.Instance.tutorialManager.TrainingTutorialComplete += skillTurnOn;
        GameManager.Instance.tutorialManager.TutorialComplete += DungeonSelecOn;
        checkTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.selectedDungeon == null)
        {
            StartDungeon.interactable = false;
        }
        else
        {
            StartDungeon.interactable = true;
        }
        if (!GameManager.Instance.tutorialActive || GameManager.Instance.tutorialManager.currentState != TutorialState.SelectDungeon)
        {
            DungeonSelection.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState == TutorialState.SelectDungeon)
        {
            DungeonSelection.gameObject.GetComponent<Animator>().enabled = true;
        }

        if (!GameManager.Instance.tutorialActive || GameManager.Instance.tutorialManager.currentState != TutorialState.UnlockSkill)
        {
            SkillsMenu.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState == TutorialState.UnlockSkill)
        {
            SkillsMenu.gameObject.GetComponent<Animator>().enabled = true;
        }

        
        if (!GameManager.Instance.tutorialActive || GameManager.Instance.tutorialManager.currentState != TutorialState.StartDungeon)
        {
            StartDungeon.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState == TutorialState.StartDungeon && GameManager.Instance.selectedDungeon != null)
        {
            StartDungeon.gameObject.GetComponent<Animator>().enabled = true;
        }

        if (!GameManager.Instance.tutorialActive || GameManager.Instance.tutorialManager.currentState != TutorialState.TrainingIntro)
        {
            Training.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (GameManager.Instance.tutorialActive && GameManager.Instance.tutorialManager.currentState == TutorialState.TrainingIntro)
        {
            Training.gameObject.GetComponent<Animator>().enabled = true;
        }

    }
    private void OnDisable()
    {
        GameManager.Instance.tutorialManager.TrainingTutorialComplete -= skillTurnOn;
        GameManager.Instance.tutorialManager.TutorialComplete -= DungeonSelecOn;
    }

    public void OnStartDungeon()
    {
        if(GameManager.Instance.selectedDungeon != null)
        {
            foreach (var character in GameManager.Instance.PartyMembers)
            {
                character.CurrentHealth = character.CurrentMaxHealth;
            }
            GameManager.Instance.LoadDungeonScene();
        }
        
    }
    public void checkTutorial()
    {
        if (GameManager.Instance.tutorialActive)
        {
            SkillsMenu.interactable = false;
            Shop.interactable = false;
            DungeonSelection.interactable = false;
        }
    }
    public void skillTurnOn()
    {
        if (GameManager.Instance.tutorialActive)
        {
            SkillsMenu.interactable = true;
        }
    }
    public void DungeonSelecOn()
    {
       
        DungeonSelection.interactable = true;
        
    }
}
