
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public TutorialState currentState = TutorialState.Start;
    public TurnManager turnManager;
    public int turnCounter = 0;
    public Sprite stunicon;

    public TutorialUIManager UIManager;
    public string textToShow;
    public Action TrainingTutorialComplete;
    public Action TutorialComplete;

    public bool firstRound = true;
    void Start()
    {
        textToShow = "";
        StartCoroutine(ChangeTextAfterTime(1.0f, "This team is struggling maybe you can help."));
        StartCoroutine(ChangeTextAfterTime(4.0f, "Start by selecting the knight by clicking on them."));
    }
    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        UIManager = FindObjectOfType< TutorialUIManager>();
        if (UIManager != null)
        {
            UIManager.Bind(textToShow);
        }
    }
    public void OnChoosingAction()
    {
        currentState = TutorialState.SelectAbility;
        if(firstRound)
        textToShow = "Select the available ability, the rest are on cooldown.";

        
    }
    public void OnSelectingTarget()
    {
        currentState = TutorialState.SelectTarget;
        if (firstRound)
        {
            textToShow = "Select Target by clicking on them and click confirm to queue the action.";
            StartCoroutine(ChangeTextAfterTime(5.0f, "After all characters have had their turn planned they execute them."));
            StartCoroutine(ChangeTextAfterTime(10.0f, "You can also click end turn to finish a round."));
            StartCoroutine(ChangeTextAfterTime(15.0f, "Keep fighting and try to win."));
            firstRound = false;
        }
            
    }
    public void OnActionResolved(PartyMember member,Ability ability, List<CombatMember> targets)
    {
        currentState = TutorialState.SelectCharacter;
  
    }
    public void subscribeToEvents()
    {
        if (turnManager != null)
        {
            turnManager.OnChoosingAction += OnChoosingAction;
            turnManager.OnSelectingTarget += OnSelectingTarget;
            turnManager.OnActionResolved += OnActionResolved;
        }
    }
    public void unsubscribeToEvents()
    {
        if (turnManager != null)
        {
            turnManager.OnChoosingAction -= OnChoosingAction;
            turnManager.OnSelectingTarget -= OnSelectingTarget;
            turnManager.OnActionResolved -= OnActionResolved;
        }
    }
    public void setUpTutorialData(TurnManager tm)
    {
        if (GameManager.Instance.tutorialActive)
        {
            turnManager = tm;
            if (turnManager != null)
            {
                subscribeToEvents();

                currentState = TutorialState.SelectCharacter;

                for (int i = 0; i < GameManager.Instance.PartyMembers[0].activeAbilities.Count; i++)
                {
                    if (i != 0)
                    {
                        GameManager.Instance.PartyMembers[0].activeAbilities[i].cooldownLeft = 2;
                    }
                }
                for (int i = 0; i < GameManager.Instance.PartyMembers.Count; i++)
                {
                    if (i != 0)
                    {

                        StunEffect effect = new StunEffect(1);
                        effect.name = "Stunned";
                        effect.description = "Target is knocked unconcious and cannot move.";
                        effect.icon = stunicon;
                        effect.colorType = colorType.Negative;

                        GameManager.Instance.PartyMembers[i].ApplyEffect(effect, false);
                        GameManager.Instance.PartyMembers[i].HasTurn = false;
                    }

                    int healthTaken = UnityEngine.Random.RandomRange(Mathf.RoundToInt(GameManager.Instance.PartyMembers[i].CurrentHealth * 0.8f), Mathf.RoundToInt(GameManager.Instance.PartyMembers[i].CurrentHealth * 0.3f));

                    GameManager.Instance.PartyMembers[i].CurrentHealth -= healthTaken;
                }
            }
        }
        
    }
    public void trainingTutorial()
    {
        currentState = TutorialState.TrainingIntro;
        UIManager = GetComponent<TutorialUIManager>();
        StopAllCoroutines();

        textToShow = "This party needs to get a bit stronger before going back to the dungeons.";
        
        StartCoroutine(ChangeTextAfterTime(5.0f, "Enter the training menu and select the strength training."));
        StartCoroutine(ChangeTextAfterTime(10.0f, "This will increase the strength stat by 1."));
        StartCoroutine(ChangeTextAfterTime(15.0f, "This training can only be done once between dungeons."));
        StartCoroutine(ChangeTextAfterTime(20.0f, "Now close this and open the character skills menu."));
    }
    public void UnlockSkill()
    {
        TrainingTutorialComplete?.Invoke();
        currentState = TutorialState.UnlockSkill;
        StopAllCoroutines();

        textToShow = "Now you can unlock a new skill by selecting it.";
        StartCoroutine(ChangeTextAfterTime(5.0f, "These are the requirements to learn this skill. Purchase one of them."));
        
    }
    public void EquipSkill()
    {
        currentState = TutorialState.EquipSkill;

        StopAllCoroutines();
        textToShow = "Drag the skill icon to any slot to equip it.";
        StartCoroutine(ChangeTextAfterTime(5.0f, "Passive skills can only be equiped in the last 2 slots."));
    }
    public void EndTutorial()
    {
        currentState = TutorialState.SelectDungeon;
        
        StopAllCoroutines();
        textToShow = "Now click the dungeon selection board and select one listed.";
        TutorialComplete?.Invoke();
    }

    public void StartDungeonTut()
    {
        currentState = TutorialState.StartDungeon;

        StopAllCoroutines();
        textToShow = "Now Enter the dungeon by clicking the Start Dungeon button.";

        
    }

    public void DungeonTraversalText()
    {
        StopAllCoroutines();
        textToShow = "Click on the room icon you wish to travel to.";
        StartCoroutine(turnOffTutorial());
    }

    public IEnumerator ChangeTextAfterTime(float time, string text)
    {
        yield return new WaitForSeconds(time);
        textToShow = text;
    }
    public IEnumerator turnOffTutorial()
    {
        yield return new WaitForSeconds(5.0f);
        GameManager.Instance.tutorialActive = false;
        GameManager.Instance.Stats.tutorial = false;
    }
}

public enum TutorialState
{
    Start,
    SelectCharacter,
    SelectAbility,
    SelectTarget,
    EndTurn,
    ForceLoss,
    TransitionToHub,
    TrainingIntro,
    UnlockSkill,
    EquipSkill,
    SelectDungeon,
    StartDungeon,
    TutorialEnd,
}