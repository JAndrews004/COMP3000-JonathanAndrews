using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public List<GameObject> CharacterPositions;
    public List<Transform> EnemyPositions;
    public List<PartyMember> PartyMembers = GameManager.Instance.PartyMembers;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RefreshPartyMembers();
        PartyMembers = GameManager.Instance.PartyMembers;
        StartCombat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCombat()
    {
        for(int i = 0; i < PartyMembers.Count; i++)
        {
            CharacterPositions[i].GetComponent<PartySlot>().CurrentPartyMember = PartyMembers[i] ;
        }

        // do the same for enemies
    }
}
