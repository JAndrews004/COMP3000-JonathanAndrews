using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;

    public DungeonRuntimeState runtimeState;

    public int EliteChance;
    public int TreasureChance;

    public void GenerateLayout(int rooms)
    {
        int startx = Random.RandomRange(0, gridWidth);
        int starty = Random.RandomRange(0, gridHeight);

        RoomState startRoom = new RoomState();
        startRoom.visitable = true;
        startRoom.cleared = true;
        startRoom.roomType = RoomType.Start;
        startRoom.currentPosition = new Vector2Int(startx, starty);

        runtimeState.rooms.Add(startRoom.currentPosition, startRoom);

        List<RoomState> openRooms = new List<RoomState>() {startRoom };

        while(runtimeState.rooms.Count <rooms && openRooms.Count !=0)
        {
            RoomState currentRoom = openRooms[Random.RandomRange(0, openRooms.Count)];

            if(Random.RandomRange(0,100) < 80)
            {
                currentRoom = openRooms[openRooms.Count-1];
            }
            
            //get open spaces around current room

            List<Vector2Int> directions = new List<Vector2Int>() {new Vector2Int(0,1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };
            List<Vector2Int> validPositions = new List<Vector2Int>();

            foreach(Vector2Int dir in directions)
            {
                Vector2Int target = currentRoom.currentPosition + dir;
                if(target.x>0 && target.y>0 && target.x <= gridWidth && target.y <= gridHeight && !runtimeState.rooms.ContainsKey(target))
                {
                    validPositions.Add(target);
                }
            }

            // pick random valid direction
            if(validPositions.Count == 0)
            {
                openRooms.Remove(currentRoom);
            }
            else
            {
                Vector2Int newPos = validPositions[Random.Range(0, validPositions.Count)];

                //create new room
                RoomState newRoom = new RoomState();
                newRoom.currentPosition = newPos;
                newRoom.visitable = false;
                newRoom.cleared = false;
                newRoom.roomType = RoomType.Normal;

                //connect and add room
                currentRoom.connectedRooms.Add(newRoom);
                newRoom.connectedRooms.Add(currentRoom);
                runtimeState.rooms.Add(newPos, newRoom);
                openRooms.Add(newRoom);
                
            }

        }

        foreach (KeyValuePair<Vector2Int, RoomState> room in runtimeState.rooms)
        {
            if (room.Value.connectedRooms.Count == 0)
            {
                runtimeState.rooms.Remove(room.Key);
            }
        }

        RoomState bossRoom = null;
        float maxDistance = -1;

        foreach(KeyValuePair<Vector2Int,RoomState> room in runtimeState.rooms)
        {
            float distance = Mathf.Sqrt((room.Key.x - startRoom.currentPosition.x) * (room.Key.x - startRoom.currentPosition.x) + (room.Key.y - startRoom.currentPosition.y) * (room.Key.y - startRoom.currentPosition.y));

            if (maxDistance < distance)
            {
                maxDistance = distance;
                bossRoom = room.Value;
            }
        }
        bossRoom.roomType = RoomType.Boss;

        foreach (KeyValuePair<Vector2Int, RoomState> room in runtimeState.rooms)
        { 
           if(room.Value.roomType == RoomType.Normal)
           {
                int roll = Random.RandomRange(0, 100);

                if(roll < EliteChance)
                {
                    room.Value.roomType = RoomType.Elite;
                }
                else if(roll < EliteChance + TreasureChance)
                {
                    room.Value.roomType = RoomType.Treasure;
                }
                else
                {
                    room.Value.roomType = RoomType.Normal;
                }
           }

        }
        

        

    }
}
