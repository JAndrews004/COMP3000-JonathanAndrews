using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DungeonData
{
    public string name;
    public CharacterClass requiredPass;
    public Length length;
    public Element mainElement;
    public Element secondaryElement;
    public int difficulty; //1,2,3,4 easy meduim hard insane
    public int expectedRewardGold;
    public int expectedRewardXP;
    public int recommendedLevel;
    public int numberOfRooms;

    private List<string> prefixes = new List<string>(){
         "Molten",
        "Forsaken",
        "Spooky",
        "Ancient",
        "Cursed",
        "Forgotten",
        "Whispering",
        "Shattered",
    };
    private List<string> suffixes = new List<string>(){
         "Manor",
        "Falls",
        "Catacombs",
        "Depths",
        "Sanctum",
        "Caverns",
        "Keep",
        "Ruins",
    };

    public void Generate(int averageLevel)
    {
        expectedRewardGold = Random.RandomRange(1, 4);
        expectedRewardXP = Random.RandomRange(1, 4);

        List<Length> enums= new List<Length>() { Length.Short,Length.Medium, Length.Long };
        length = enums[Random.RandomRange(0, 3)];

        switch (length)
        {
            case Length.Short:
                numberOfRooms = Random.RandomRange(6, 9);
                break;
            case Length.Medium:
                numberOfRooms = Random.RandomRange(10, 15);
                break;
            case Length.Long:
                numberOfRooms = Random.RandomRange(15, 20);
                break;
        }
        List<Element> ElEnums = new List<Element>() { Element.Fire, Element.Air, Element.Water,Element.Earth };
        if (averageLevel >= 15)
        {
            mainElement = ElEnums[Random.RandomRange(0, 4)];
        }
        if (averageLevel >= 30)
        {
            secondaryElement = ElEnums[Random.RandomRange(0,4)];
            while(mainElement == secondaryElement)
            {
                secondaryElement = ElEnums[Random.RandomRange(0, 4)];
            }
        }
        difficulty = Random.RandomRange(1, 4);

        int easyOffset = 5;
        int hardOffset = 5;
        int insaneOffset = 10;
        switch (difficulty)
        {
            case 1:
                recommendedLevel = averageLevel + Random.RandomRange(-2,2)+ easyOffset;
                
                break;
            case 2:
                recommendedLevel = averageLevel + Random.RandomRange(-2,2);
                break;
            case 3:
                recommendedLevel = averageLevel + Random.RandomRange(-2, 2) + hardOffset;
                break;
            case 4:
                recommendedLevel = averageLevel + Random.RandomRange(-2, 2)+ insaneOffset;
                break;
        }
        if(recommendedLevel <= 0)
        {
            recommendedLevel = 0;
        }
        name = prefixes[Random.RandomRange(0, prefixes.Count)] +" " + suffixes[Random.RandomRange(0, suffixes.Count)];

        int passLevel = averageLevel + Random.RandomRange(0, 5);

        switch (passLevel)
        {
            case < 10:
                requiredPass = CharacterClass.F;
                break;
            case < 20:
                requiredPass = CharacterClass.E;
                break;
            case < 30:
                requiredPass = CharacterClass.D;
                break;
            case < 40:
                requiredPass = CharacterClass.C;
                break;
            case < 50:
                requiredPass = CharacterClass.B;
                break;
            case < 60:
                requiredPass = CharacterClass.A;
                break;
            case <= 60:
                requiredPass = CharacterClass.S;
                break;
        }
    }
}

