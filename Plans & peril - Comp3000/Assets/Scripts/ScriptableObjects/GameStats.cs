using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameStats")]
public class GameStats : ScriptableObject
{
    public int Gold;
    public int passLevel;
    public bool tutorial = true;
}
