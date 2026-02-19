using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DebugManager : MonoBehaviour
{
    bool activate = false;

    public CombatManager cm;
    public TurnManager tm;

    public Canvas debugCanvas;
    public TextMeshProUGUI combatStateText;
    public TextMeshProUGUI combatLogText;

    public List<TextMeshProUGUI> CharacterOverlay;
    public List<TextMeshProUGUI> EnemyOverlay;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)&&activate == false)
        {
            activate = true;
        }
        else if ((Input.GetKeyDown(KeyCode.F1) && activate == true))
        {
            activate = false;
        }

        debugCanvas.gameObject.SetActive(activate);

        if (cm != null)
        {
            string phaseState = tm.playerPhase ? "Player phase" : "EnemyPhase";
            int aliveCharacters = 0;
            foreach(PartyMember pm in cm.PartyMembers)
            {
                if (pm.Alive)
                {
                    aliveCharacters++;
                }
            }
            int aliveEnemies = 0;
            foreach (EnemyMember em in tm.EnemyMembers)
            {
                if (em.Alive)
                {
                    aliveEnemies++;
                }
            }
            combatStateText.text = $"<color=#1ABC9C>{phaseState}\n Alive Characters:{aliveCharacters}\n Alive Enemies:{aliveEnemies}\n</color>";

            for(int  i = 0; i < 4; i++)
            {
                string statusEffectNames;
                statusEffectNames = string.Join(", ", cm.PartyMembers[i].activeEffects.Select(t => $"{t.name}"));
                CharacterOverlay[i].text = $"<color=#2ECC71>HP:{cm.PartyMembers[i].CurrentHealth}\n Max HP:{cm.PartyMembers[i].CurrentMaxHealth}\n</color><color=#C0392B> Alive:{cm.PartyMembers[i].Alive}\n</color><color=#F1C40F> Status effects:{statusEffectNames} \n</color><color=#3498DB> Shield value:{cm.PartyMembers[i].shieldValue}\n</color>";
            }
            for (int i = 0; i < tm.EnemyMembers.Count; i++)
            {
                string statusEffectNames;
                statusEffectNames = string.Join(", ", tm.EnemyMembers[i].activeEffects.Select(t => $"{t.name}"));
                EnemyOverlay[i].text = $"<color=#2ECC71>HP:{tm.EnemyMembers[i].CurrentHealth}\n Max HP:{tm.EnemyMembers[i].CurrentMaxHealth}\n</color><color=#C0392B> Alive:{tm.EnemyMembers[i].Alive}\n</color><color=#F1C40F> Status effects:{statusEffectNames} \n</color><color=#3498DB> Shield value:{tm.EnemyMembers[i].shieldValue}\n</color>";
            }

        }
    }
    public void setDebugLogText(debugCombatLog data)
    {
        if (data.attacker != null && data.targets != null && data.abilityUsed != null)
        {
            string targetLines = string.Join("\n", data.targets.Select(t =>
            {
                int raw = data.rawDamages[t];
                int received = data.damageReceived[t];
                return $"<color=#FF0000>{t.baseStats.characterName}</color> - Raw: {raw}, Received: {received}";
            }));

            combatLogText.text = $"<color=#E0E0E0>Attacker: {data.attacker.baseStats.characterName}</color>\n" +
                                 $"<color=#E0E0E0>Ability used: {data.abilityUsed.AbilityData.abilityName}</color>\n" +
                                 targetLines;
        }
    }

}

public struct debugCombatLog
{
    public CombatMember attacker;
    public List<CombatMember> targets;
    public Ability abilityUsed;
    public Dictionary<CombatMember,int> rawDamages;
    public Dictionary<CombatMember, int> damageReceived;
    
}