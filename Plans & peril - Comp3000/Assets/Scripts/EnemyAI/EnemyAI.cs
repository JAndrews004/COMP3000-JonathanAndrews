using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour 
{
    public abstract Turn ChooseAction(EnemyMember enemy);

    public List<CombatMember> GetAliveEnemies()
    {
        List<CombatMember> enemies = new List<CombatMember> { };

        foreach (EnemyMember enemy in GameManager.Instance.EnemyMembers)
        {
            if (enemy != null && enemy.Alive)
            {
                enemies.Add(enemy);
            }

        }
        return enemies;

    }

    public List<CombatMember> GetAlivePlayers()
    {
        List<CombatMember> Characters = new List<CombatMember> { };

        foreach (PartyMember character in GameManager.Instance.PartyMembers)
        {
            if (character != null && character.Alive)
            {
                Characters.Add(character);
            }

        }
        return Characters;

    }
}
