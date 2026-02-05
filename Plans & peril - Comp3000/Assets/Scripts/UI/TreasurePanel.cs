using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasurePanel : MonoBehaviour
{
    public Button closeButton;
    public TextMeshProUGUI goldText;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            closePanel();

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closePanel()
    {
        gameObject.SetActive(false);
    }
    public void setGoldText(int gold)
    {
        goldText.text = $"You found {gold} gold!";
    }
}
