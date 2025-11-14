using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLogManager : MonoBehaviour
{
    public List<TextMeshProUGUI> messageList;
    

    public void AddMessage(string message)
    {
       for (int i = messageList.Count - 1; i > 0; i--)
        {
            
           messageList[i].text = messageList[i-1].text;
 
        }
        messageList[0].text = message;
    }
    public void ClearAllMessages()
    {
        foreach(TextMeshProUGUI msg in messageList)
        {
            msg.text = "";
        }
    }

}
