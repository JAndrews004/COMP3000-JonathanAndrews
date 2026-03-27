using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatusIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public TextMeshProUGUI durationText;
    private Effect linkedEffect;
    

    public void Setup(Effect effect)
    {
        linkedEffect = effect;
        if(effect.icon == null)
        {
            Destroy(gameObject);
        }
        if(effect.icon != null)
        {
            iconImage.sprite = effect.icon;
        }
        
        durationText.text = effect.duration.ToString();

        switch (effect.colorType)
        {
            case colorType.Positive:
                iconImage.color = Color.green;
                break;
            case colorType.Negative:
                iconImage.color = Color.red;
                break;
            case colorType.Neutral:
                iconImage.color = Color.blue;
                break;
        }
    }

    public void UpdateDuration()
    {
        durationText.text = linkedEffect.duration.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.6f;
        Vector3 tooltipPos = Input.mousePosition + new Vector3(10, -10, 0);
        StatusTooltip.ShowTooltip(linkedEffect.name, linkedEffect.description, tooltipPos);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one *1.5f;
        StatusTooltip.HideTooltip();

    }
}
