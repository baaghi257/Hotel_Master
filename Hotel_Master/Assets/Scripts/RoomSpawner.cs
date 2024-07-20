using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> rooms = new List<GameObject>();
    [SerializeField] GameObject roomPrefab;
    [SerializeField] float roomRotationValue = 0f;

    public static RoomSpawner instance;
    public bool isNewRoomSpawned = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Data data = SaveSystem.LoadGame();
        if(data != null && data.allRoomSpawnerData.Count > 0 )
        {
            int spawnerIndex = RoomSpawner.instance == this ? 0 : 1;
            LoadRooms(data.allRoomSpawnerData[spawnerIndex].roomsData);
            Debug.Log("LoadRooms");
        }
        else
        {
            Debug.Log("SpawnRooms");
            SpawnRoom();
        }
    }


    private void Update()
    {
        AddNewRoom();
    }

    public void LoadRooms(List<RoomData> roomData)
    {
        foreach(RoomData room in roomData)
        {
            GameObject r = Instantiate(roomPrefab, room.position, room.rotation);
            rooms.Add(r);
        }
    }
    public void SpawnRoom()
    {
        GameObject room = Instantiate(roomPrefab, transform.position, Quaternion.identity);
        room.transform.Rotate(0, roomRotationValue, 0);
        rooms.Add(room);
    }
    public void AddNewRoom()
    {
        if (rooms.Count > 0 && rooms[rooms.Count - 1].GetComponentInChildren<Room>().canSpawnNewRoom == true && isNewRoomSpawned == false)
        {
            GameObject newRoom = Instantiate(roomPrefab, rooms[rooms.Count - 1].transform.position + new Vector3(-7,0,0), Quaternion.identity);
            newRoom.transform.Rotate(0, roomRotationValue, 0);
            rooms.Add(newRoom);
            isNewRoomSpawned = true;
        }
    }
}
