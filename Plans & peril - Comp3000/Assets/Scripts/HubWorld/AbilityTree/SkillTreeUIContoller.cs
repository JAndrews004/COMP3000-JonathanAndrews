using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUIContoller : MonoBehaviour
{
    public GameObject TreeColumn;
    public GameObject SkillButton;
    public GameObject linePrefab;
    public GameObject lineManager;
    public GameObject Content;
    public Dictionary<AbilityData, SkillButton> skillButtonPairs = new Dictionary<AbilityData, SkillButton>();

    public GameObject canvas;
    public GameObject floatingIconPrefab;

    List<GameObject> lines = new List<GameObject>();
    List<GameObject> Columns = new List<GameObject>();
    Dictionary<(AbilityData prereq, AbilityData ability), Image> connectionLines = new Dictionary<(AbilityData, AbilityData), Image>();
    public void generateSkillTree(PartyMember member)
    {
        clearSkillTree();
        List<AbilityData> CharacterAbilitySet = member.ALLUNLOCKABLEABILITIES;
        CharacterSkillTree characterSkillTree = member.characterSkillTree;
        int maxcol = 0;
        foreach (AbilityData ability in CharacterAbilitySet)
        {
            if (maxcol < ability.treeCol)
            {
                maxcol = ability.treeCol;
            }
        }
        for (int i = 0; i <= maxcol; i++)
        {
            //create column
            Columns.Add(Instantiate(TreeColumn, Content.transform));

        }

        foreach (AbilityData ability in CharacterAbilitySet)
        {

            GameObject currentButton = Instantiate(SkillButton, Columns[ability.treeCol].transform);
            currentButton.transform.SetSiblingIndex(ability.treeRow);
            currentButton.GetComponent<Image>().sprite = ability.icon;
            currentButton.GetComponent<SkillButton>().canvas = canvas;
            currentButton.GetComponent<SkillButton>().floatingIconPrefab = floatingIconPrefab;
            currentButton.GetComponent<SkillButton>().ability = ability;
            currentButton.GetComponent<SkillButton>().rectTransform = currentButton.GetComponent<RectTransform>();
            if (ability.unlocked)
            {
                Transform overlay = currentButton.transform.Find("Overlay");
                if (overlay != null)
                {
                    overlay.gameObject.SetActive(false); // hide child only
                }
            }
            SkillButton current = currentButton.GetComponent<SkillButton>();
            if (!skillButtonPairs.ContainsKey(ability))
            {
                skillButtonPairs.Add(ability, current);
            }       
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(Content.GetComponent<RectTransform>());

        //creating connection lines between the skills
        foreach (AbilityData ability in CharacterAbilitySet)
        {
            foreach (AbilityData prereq in ability.prerequisiteAbilities)
            {
                GameObject line = Instantiate(linePrefab, lineManager.transform);

                RectTransform startRect = skillButtonPairs[prereq].rectTransform;
                RectTransform endRect = skillButtonPairs[ability].rectTransform;
                RectTransform lineRect = line.GetComponent<RectTransform>();
                RectTransform lineParentRect = lineManager.GetComponent<RectTransform>();


                Vector3 startWorld = startRect.TransformPoint(startRect.rect.center);
                Vector3 endWorld = endRect.TransformPoint(endRect.rect.center);

                Vector2 startPos = lineParentRect.InverseTransformPoint(startWorld);
                Vector2 endPos = lineParentRect.InverseTransformPoint(endWorld);




                // Direction and distance
                Vector3 dir = endPos - startPos;
                float distance = dir.magnitude;

                // Midpoint
                lineRect.anchoredPosition = (startPos + endPos) * 0.5f;

                //setting width (length) leaving height (thickness of line)
                lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);

                // Set rotation
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                lineRect.localRotation = Quaternion.Euler(0, 0, angle);

                // Set color
                Image img = line.GetComponent<Image>();
                if (!prereq.unlocked) img.color = Color.gray;
                else if (!ability.unlocked) img.color = Color.yellow;
                else img.color = Color.green;

                lines.Add(line);
                connectionLines.Add((prereq, ability), img);
            }
        }


    }
    public void clearSkillTree()
    {
        
        foreach(var col in Columns)
        {
            DestroyImmediate(col.gameObject);
        }
        Columns = new List<GameObject>();
        foreach (var line in lines)
        {
            DestroyImmediate(line.gameObject);
        }
        RectTransform lm = lineManager.GetComponent<RectTransform>();
        lm.anchoredPosition = Vector2.zero;
        lm.localPosition = Vector3.zero;
        lm.localRotation = Quaternion.identity;
        lm.localScale = Vector3.one;

        lines = new List<GameObject>();
        skillButtonPairs = new Dictionary<AbilityData, SkillButton>();
        connectionLines = new Dictionary<(AbilityData, AbilityData), Image>();
        Content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    }

    public void updateSkillTree()
    {
        foreach (var pair in skillButtonPairs)
        {
            AbilityData ability = pair.Key;
            SkillButton button = pair.Value;

            Transform overlay = button.transform.Find("Overlay");
            if (overlay != null)
            {
                overlay.gameObject.SetActive(!ability.unlocked);
            }
        }
        foreach (var pair in connectionLines)
        {
            AbilityData prereq = pair.Key.prereq;
            AbilityData ability = pair.Key.ability;
            Image lineImage = pair.Value;

            if (!prereq.unlocked)
                lineImage.color = Color.gray;
            else if (!ability.unlocked)
                lineImage.color = Color.yellow;
            else
                lineImage.color = Color.green;
        }
    }

}
