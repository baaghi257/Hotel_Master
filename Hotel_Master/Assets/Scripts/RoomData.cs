using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isUnLocked;
    public RoomData(Vector3 pos,  Quaternion rot, bool unLocked)
    {
        position = pos;
        rotation = rot;
        isUnLocked = unLocked;
    }
}

[System.Serializable]
public class RoomSpawnerData
{
    public List<RoomData> roomsData = new List<RoomData>();
}
