using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomState
{
    public Vector2Int currentPosition;

    public RoomType roomType;
    public bool visitable;
    public bool cleared;

    public List<RoomState> connectedRooms = new List<RoomState>();

}

public enum RoomType
{
    Start,
    Normal,
    Elite,
    Treasure,
    Boss,
}