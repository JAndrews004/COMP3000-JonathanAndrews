using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityToolTip : MonoBehaviour
{
    public TextMeshProUGUI EffectNameText;
    public TextMeshProUGUI EffectDescriptionText;
    public CanvasGroup Group;

    private static AbilityToolTip instance;

    private void Awake()
    {
        instance = this;
        HideTooltip();
    }

    public static void ShowTooltip(string name, string description, Vector3 position)
    {
        if (instance == null) return;

        instance.EffectNameText.text = name;
        instance.EffectDescriptionText.text = description;

        // Convert screen position to UI space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            instance.transform.parent as RectTransform,
            position,
            null,
            out Vector2 localPoint
        );
        instance.GetComponent<RectTransform>().localPosition = localPoint;

        instance.Group.alpha = 1;
        instance.Group.blocksRaycasts = false;
    }


    public static void HideTooltip()
    {
        if (instance == null) return;
        instance.Group.alpha = 0;
        instance.Group.blocksRaycasts = false;
    }
}
