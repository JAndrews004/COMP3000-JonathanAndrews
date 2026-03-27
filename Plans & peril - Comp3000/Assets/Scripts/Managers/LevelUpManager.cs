using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LevelUpManager : MonoBehaviour
{

    public List<PartyMember> leveledUpCharacters = new List<PartyMember>() { };
    public List<LevelUpViewModel> activePannels = new List<LevelUpViewModel>() { };
    

    public GameObject LevelUpPrefab;

    private void Awake()
    {
        foreach(PartyMember mem in GameManager.Instance.PartyMembers)
        {
            mem.levelUp += OnLevelUp;
        }
    }
    void OnDisable()
    {
        foreach (PartyMember mem in GameManager.Instance.PartyMembers)
        {
            mem.levelUp -= OnLevelUp;
        }
    }
    public void StartLevelUpSequence()
    {
        leveledUpCharacters = leveledUpCharacters.Distinct().ToList();
        foreach (PartyMember mem in leveledUpCharacters)
        {
            GameObject levelUpPrefab = Instantiate(LevelUpPrefab, this.transform);
            LevelUpViewModel vm = new LevelUpViewModel(mem, levelUpPrefab);
            vm.OnClosed += HandlePanelClose;


            levelUpPrefab.GetComponent<LevelUpView>().Bind(vm);
            activePannels.Add(vm);
        }

        
    }
    public void HandlePanelClose(LevelUpViewModel vm)
    {
        Destroy(vm.UiObject);
        activePannels.Remove(vm);
        
        vm.OnClosed -= HandlePanelClose;
        
    }

    public void OnLevelUp(PartyMember mem)
    {
        if (!leveledUpCharacters.Contains(mem))
        {
            leveledUpCharacters.Add(mem);
        }

    }
}
