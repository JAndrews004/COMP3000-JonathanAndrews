using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionOrderQueue : MonoBehaviour
{
    public List<Image> characterImages = new List<Image> { };

    public List<Image> Arrows;
    public Sprite transparent;
    private List<bool> imageFilled = new List<bool>() {false,false,false,false };

    public TurnManager turnManager;
    // Start is called before the first frame update
    void Awake()
    {
        turnManager.OnActionResolved += ActionResolved;
    }
    private void OnDestroy()
    {
        turnManager.OnActionResolved -= ActionResolved;
    }
    // Update is called once per frame
    void Update()
    {
        if (!turnManager.playerPhase)
        {
            resetQueue();
        }
    }

    public void ActionResolved(PartyMember mem, Ability a, List<CombatMember> t)
    {
        for(int i = 0; i < imageFilled.Count; i++)
        {
            if (!imageFilled[i])
            {
                imageFilled[i] = true;
                characterImages[i].sprite = mem.baseStats.HeadShot;
                if (i > 0)
                {
                    Arrows[i-1].gameObject.SetActive(true);
                }
                break;
               
            }
        }
    }

    public void resetQueue()
    {
        imageFilled = new List<bool>() { false, false, false, false };
        for (int i = 0; i < characterImages.Count; i++)
        {
            characterImages[i].sprite = transparent;
        }
        foreach(Image arrow in Arrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }
}
