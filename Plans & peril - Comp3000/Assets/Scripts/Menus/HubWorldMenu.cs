using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubWorldMenu : MonoBehaviour
{
    public Button menuButton;
    public Button closeButton;
    public Button quitButton;
    public GameObject menuPanel;
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            menuPanel.SetActive(false);
        });
        quitButton.onClick.AddListener(() => { 
            Application.Quit();
        });
        menuButton.onClick.AddListener(() => {
            menuPanel.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
