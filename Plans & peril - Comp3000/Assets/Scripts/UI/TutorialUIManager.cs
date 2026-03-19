using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{

    public GameObject UIBox;
    public TextMeshProUGUI UIText;
    
    public void Start()
    {
        if (!GameManager.Instance.tutorialActive)
        {
            UIBox.SetActive(false);
        }
    }
    public void Bind(string text)
    {
        UIText.text = text;
    }
}
