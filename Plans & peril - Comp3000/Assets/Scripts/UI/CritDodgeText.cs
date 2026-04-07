
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CritDodgeText : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float speed;
    public float scaleSpeed;
    public float opacitySpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += new Vector3(speed *Time.deltaTime, speed * Time.deltaTime, 0);
        this.transform.localScale *= (scaleSpeed * Time.deltaTime)+1;

        if (text.color.a - opacitySpeed * Time.deltaTime >= 0)
        {
            text.color = new Vector4(text.color.r, text.color.b, text.color.g, text.color.a - opacitySpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
