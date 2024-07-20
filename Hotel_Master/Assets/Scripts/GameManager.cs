using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] public int totalCoins = 100;
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LoadGameData();
    }

    private void OnApplicationQuit()
    {
        List<RoomSpawner> roomSpwner = new List<RoomSpawner>(FindObjectsOfType<RoomSpawner>());
        SaveAllRoomUnlockStates(roomSpwner);
        SaveSystem.SaveGameCoins(this, roomSpwner, CounterDesk.instance.hasPlayerSpawned, Player.instance.transform.position);
    }
    private void Start()
    {
        coinText.text = "Coins : " + totalCoins;
    }
    public void BuyCoins(int value)
    {
        StartCoroutine(AnimateCoinChange(totalCoins, totalCoins + value));
        totalCoins += value;
    }

    public void SpendCoins(int value)
    {
        StartCoroutine(AnimateCoinChange(totalCoins, totalCoins - value));
        totalCoins -= value;
    }

    private IEnumerator AnimateCoinChange(int startValue, int endValue)
    {
        float duration = 1.0f; // Duration of the animation in seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, elapsedTime / duration));
            coinText.text = "Coins : " + currentValue;
            yield return null;
        }

        coinText.text = "Coins : " + endValue;
    }

    private void LoadGameData()
    {
        Data data = SaveSystem.LoadGame();
        List<RoomSpawner> roomSpwner = new List<RoomSpawner>(FindObjectsOfType<RoomSpawner>());
        LoadAllRoomUnlockStates(roomSpwner);
        totalCoins = data.coins;
        coinText.text = "Coin : " + data.coins;
        CounterDesk.instance.hasPlayerSpawned = data.hasCounterPrefabSpawned;
        Player.instance.transform.position = data.playerPosition;
    }

    public void SaveAllRoomUnlockStates(List<RoomSpawner> roomSpawns)
    {
        foreach(RoomSpawner spawner in roomSpawns)
        {
            for(int i = 0; i < spawner.rooms.Count; i++)
            {
                UpgradeRoom upgradeRoom = spawner.rooms[i].GetComponent < UpgradeRoom>();
                if(upgradeRoom != null)
                {
                    SaveSystem.SaveRoomUnlockState(upgradeRoom.roomId, upgradeRoom.isLocked);
                }
            }
        }
    }

    public void LoadAllRoomUnlockStates(List<RoomSpawner> roomSpawners)
    {
        foreach(RoomSpawner spawner in roomSpawners)
        {
            for(int i = 0; i < spawner.rooms.Count; i++)
            {

                UpgradeRoom upgradeRoom = spawner.rooms[i].GetComponent< UpgradeRoom>();    
                if(upgradeRoom != null)
                {
                    bool isUnlocked = SaveSystem.LoadRoomUnlockState(upgradeRoom.roomId);
                    upgradeRoom.isLocked = isUnlocked;


                    if (isUnlocked)
                    {
                        upgradeRoom.lockCanvas.SetActive(false);
                        upgradeRoom.upgradeText.text = "UPGRADE";

                    }
                    else
                    {
                        upgradeRoom.lockCanvas.SetActive(true);
                        upgradeRoom.upgradeText.text = "UNLOCK";
                    }
                }
            }
        }
    }
}
