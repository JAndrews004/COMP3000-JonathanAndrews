using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSelection : MonoBehaviour
{
    public Button closeButton;
    public GameObject DungeonEntryPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        int averageLevel = 0;
        foreach(PartyMember mem in GameManager.Instance.PartyMembers)
        {
            averageLevel += mem.level;
        }

        DungeonData data = new DungeonData();
        data.Generate(averageLevel / 4);
        GameObject newPrefab = Instantiate(DungeonEntryPrefab, gameObject.transform);
        newPrefab.GetComponent<DungeonBoardEntry>().Bind(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
