using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    public AbilityData ability;
    public RectTransform rectTransform;
    public GameObject floatingIconPrefab;
    public GameObject floatingIcon;
    public GameObject canvas;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(ability != null)
        {
            if (ability.unlocked)
            {
                //create floating icon (prefab with alpha lower) and set floating icons image to ability.icon
                floatingIcon = Instantiate(floatingIconPrefab,canvas.transform);
                floatingIcon.GetComponent<Image>().sprite = ability.icon;
                floatingIcon.GetComponent<Image>().raycastTarget = false;
            }
        }
        

    }

    public void OnDrag(PointerEventData eventData)
    {
        //move floating icon with the mouse
        if (floatingIcon !=null)
        {
            floatingIcon.transform.position = eventData.position;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(floatingIcon);
        floatingIcon = null;
    }

    public void OnClick()
    {
        gameObject.GetComponentInParent<AbilityUnlockManager>().setAbilityUI(ability);
        Debug.Log("Clicked");
    }
}
