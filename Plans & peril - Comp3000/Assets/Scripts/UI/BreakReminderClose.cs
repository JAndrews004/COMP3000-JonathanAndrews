using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakReminderClose : MonoBehaviour
{
    public void CloseReminder()
    {
        Destroy(gameObject);
    }
}
