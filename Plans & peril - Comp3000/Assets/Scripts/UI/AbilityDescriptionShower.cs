using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDescriptionShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityData linkedAbility;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(showDesc());

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        StopAllCoroutines();
        AbilityToolTip.HideTooltip();
        
    }

    public IEnumerator showDesc()
    {
        yield return new WaitForSeconds(2.0f);
        AbilityToolTip.ShowTooltip(linkedAbility.abilityName, linkedAbility.description, Input.mousePosition - new Vector3(0,-100,0));

    }
}
