using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DungeonRuntimeState : MonoBehaviour
{
    public static DungeonRuntimeState Instance { get; private set; }
    public DungeonData currentData;
    public DungeonGenerator dungeonGenerator;
    public Vector2Int currentRoom;
    
    public Dictionary<Vector2Int, RoomState> rooms = new Dictionary<Vector2Int, RoomState>();
    public Dictionary<RoomState, GameObject> roomObjects = new Dictionary<RoomState, GameObject>();
    public GameObject roomPrefab;
    public List<RoomState> visitableRoom = new List<RoomState>();
    public float roomOffset;

    public GameObject treasurePanel;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    public void OnDestroy()
    {
        foreach (var room in roomObjects)
        {
            room.Value.GetComponent<RoomNodeUI>().ButtonPressed -= roomSelected;
        }
    }
    public void roomSelected(GameObject room)
    {
        if (room.GetComponent<RoomNodeUI>().roomState.cleared)
        {
            return;
        }
        //move current room to selected
        currentRoom = room.GetComponent<RoomNodeUI>().roomState.currentPosition;
        if (room.GetComponent<RoomNodeUI>().roomState.roomType == RoomType.Treasure)
        {
            int goldToAdd = Mathf.RoundToInt(300 + currentData.recommendedLevel * 200 + UnityEngine.Random.Range(0, currentData.recommendedLevel * 50) * currentData.expectedRewardGold-1/5);
            GameManager.Instance.AddGold(goldToAdd);
            treasurePanel = FindInactiveObject<TreasurePanel>().gameObject;
            treasurePanel.GetComponent<TreasurePanel>().setGoldText(goldToAdd);
            treasurePanel.SetActive(true);
            room.GetComponent<RoomNodeUI>().roomState.visitable = false;
            room.GetComponent<RoomNodeUI>().roomState.cleared = true;
            destroyRooms();
            DrawRooms();
        }
        else if(room.GetComponent<RoomNodeUI>().roomState.roomType == RoomType.Start)
        {
            //do nothing
        }
        else
        {
            room.GetComponent<RoomNodeUI>().roomState.visitable = false;
            room.GetComponent<RoomNodeUI>().roomState.cleared = true;
            GameManager.Instance.dungeonRuntimeState = this;
            GameManager.Instance.StartCombat();
        }
        
        Debug.Log("Room clicked");
    }

    public void GenerateRooms()
    {
        dungeonGenerator.GenerateLayout(currentData.numberOfRooms);
    }

    public void DrawRooms()
    {
        if (FindInactiveObject<TreasurePanel>())
        {
            treasurePanel = FindInactiveObject<TreasurePanel>().gameObject;
        }
        if (treasurePanel)
        {
            //treasurePanel.SetActive(false);
        }
        foreach (var room in rooms)
        {

            GameObject roomObj = Instantiate(roomPrefab, this.transform);
            roomObj.GetComponent<RoomNodeUI>().roomState = room.Value;
            roomObjects.Add(room.Value, roomObj);

        }
        foreach (var room in roomObjects)
        {
            room.Value.transform.position = new Vector3(room.Key.currentPosition.x * roomOffset, room.Key.currentPosition.y * roomOffset, 0);
            if (room.Key.cleared)
            {
                foreach (RoomState connected in room.Key.connectedRooms)
                {
                    if (!connected.cleared)
                    {
                        visitableRoom.Add(connected);
                        connected.visitable = true;
                    }
                }
            }
            if (room.Key.roomType == RoomType.Start)
            {
                currentRoom = room.Key.currentPosition;
                room.Value.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (room.Key.roomType == RoomType.Boss)
            {
                SpriteRenderer sr = room.Value.GetComponent<SpriteRenderer>();
                sr.color = Color.red;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
            }
            else if (room.Key.roomType == RoomType.Treasure)
            {
                SpriteRenderer sr = room.Value.GetComponent<SpriteRenderer>();
                sr.color = Color.yellow;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
            }
            else if (room.Key.roomType == RoomType.Elite)
            {
                SpriteRenderer sr = room.Value.GetComponent<SpriteRenderer>();
                sr.color = Color.blue;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
            }
            else if (room.Key.roomType == RoomType.Normal)
            {
                SpriteRenderer sr = room.Value.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
            }
            
        }
        foreach (var room in visitableRoom)
        {
            if (!room.cleared)
            {
                SpriteRenderer sr = roomObjects[room].GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            }
            else
            {
                SpriteRenderer sr = roomObjects[room].GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
            }
        }
        foreach (var room in roomObjects)
        {
            room.Value.GetComponent<RoomNodeUI>().ButtonPressed += roomSelected;
        }
    }
    public void destroyRooms()
    {
        foreach (var room in roomObjects)
        {
            Destroy(room.Value);
        }
        roomObjects = new Dictionary<RoomState, GameObject>();
    }
    public void ResetDungeonLayout()
    {
        visitableRoom = new List<RoomState>();
        rooms = new Dictionary<Vector2Int, RoomState>();
        roomObjects = new Dictionary<RoomState, GameObject>();
        currentRoom = new Vector2Int();
    }
    public static T FindInactiveObject<T>() where T : Component
    {
        T[] all = Resources.FindObjectsOfTypeAll<T>();
        foreach (T t in all)
        {
            if (!t.gameObject.activeInHierarchy) // only inactive
                return t;
        }
        return null;
    }
}


