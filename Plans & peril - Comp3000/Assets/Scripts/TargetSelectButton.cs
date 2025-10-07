using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectButton : MonoBehaviour
{
    public EnemyMember Enemy;
    public TurnManager TurnManager;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponentInParent<EnemySlot>().CurrentEnemyMember; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy == null && GetComponentInParent<EnemySlot>().CurrentEnemyMember != null)
        {
            Enemy = GetComponentInParent<EnemySlot>().CurrentEnemyMember;
        }
    }

    public void OnCLick()
    {
        TurnManager.PlayerSelectedTarget(Enemy);
    }
}
