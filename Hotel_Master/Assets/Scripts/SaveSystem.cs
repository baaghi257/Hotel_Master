using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGameCoins(GameManager manager, List<RoomSpawner> roomSpawner, bool counterDeskSpawned, Vector3 playerpos)
    {
        try
        {
            Data data = new Data(manager, roomSpawner, counterDeskSpawned, playerpos);
            string json = JsonUtility.ToJson(data, true);
            string path = Application.persistentDataPath + "/data.json";
            File.WriteAllText(path, json);
            Debug.Log("Game data saved to " + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
        }
    }

    public static Data LoadGame()
    {
        string path = Application.persistentDataPath + "/data.json";
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                Data data = JsonUtility.FromJson<Data>(json);
                Debug.Log("Game data loaded from " + path);
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load game data: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.Log("File not found at " + path);
            return null;
        }
    }

    public static void SaveRoomUnlockState(string roomId, bool unlocked)
    {
        try
        {
            PlayerPrefs.SetInt(roomId + "_Unlocked", unlocked ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("Room unlock state saved for " + roomId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save room unlock state: " + e.Message);
        }
    }

    public static bool LoadRoomUnlockState(string roomId)
    {
        try
        {
            bool unlocked = PlayerPrefs.GetInt(roomId + "_Unlocked", 0) == 1;
            Debug.Log("Room unlock state loaded for " + roomId + ": " + unlocked);
            return unlocked;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load room unlock state: " + e.Message);
            return false;
        }
    }
}
