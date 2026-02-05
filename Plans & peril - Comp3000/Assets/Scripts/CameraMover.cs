using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    void Update()
    {
        if(GameManager.Instance !=null && GameManager.Instance.dungeonRuntimeState !=null && GameManager.Instance.dungeonRuntimeState.currentRoom != null)
        {
            Vector3 targetPosition = new Vector3(GameManager.Instance.dungeonRuntimeState.currentRoom.x * GameManager.Instance.dungeonRuntimeState.roomOffset, GameManager.Instance.dungeonRuntimeState.currentRoom.y * GameManager.Instance.dungeonRuntimeState.roomOffset, gameObject.transform.position.z);
            transform.position = targetPosition;
        }
        
    }
}
