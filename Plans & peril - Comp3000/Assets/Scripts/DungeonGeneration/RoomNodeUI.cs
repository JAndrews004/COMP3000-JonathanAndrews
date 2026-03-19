using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNodeUI : MonoBehaviour
{
    public Button button;
    public RoomState roomState;
    public Action<GameObject> ButtonPressed;
    public Image icon;
    public void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ButtonPressed.Invoke(gameObject);
        });
    }
}
