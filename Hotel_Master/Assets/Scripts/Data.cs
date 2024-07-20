using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public int coins;
    public bool isRoomLocked;
    public List<RoomSpawnerData> allRoomSpawnerData = new List<RoomSpawnerData>();
    public bool hasCounterPrefabSpawned;
    public Vector3 playerPosition;
    public Data(GameManager gameManager, List<RoomSpawner> roomSpawners, bool counterDeskSpawned, Vector3 playerPos)
    {
        coins = gameManager.totalCoins;
        hasCounterPrefabSpawned = counterDeskSpawned;
        foreach (RoomSpawner spawner in roomSpawners)
        {
            RoomSpawnerData spawnerData = new RoomSpawnerData();
            foreach (GameObject room in spawner.rooms)
            {
                UpgradeRoom upgradeRoom = room.GetComponent<UpgradeRoom>();
                spawnerData.roomsData.Add(new RoomData(room.transform.position, room.transform.rotation, upgradeRoom != null && !upgradeRoom.isLocked));
            }
            allRoomSpawnerData.Add(spawnerData);
        }

        this.playerPosition = playerPos;
    }

}


