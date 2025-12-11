using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBoardEntry
{
    string name;
    CharacterClass tier;
    Length length;
    List<Elements> elements;
    int difficulty;
    int experctedRewardGold;
    int experctedRewardXp;
    int lowerRangeLevel;
    int recommendedLevel;
    int upperRangeLevel;
    bool locked;


}
enum Length
{
    Short,
    Medium,
    Long,
}
enum Elements
{
    Air,
    Fire,
    Water,
    Earth,
}