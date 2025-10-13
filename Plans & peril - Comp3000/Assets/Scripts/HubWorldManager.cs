using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
