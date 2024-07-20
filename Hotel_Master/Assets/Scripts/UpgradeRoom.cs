using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UpgradeRoom : MonoBehaviour
{
    [SerializeField] Image fillImg;
    [SerializeField] GameObject room;
    [SerializeField] public GameObject lockCanvas;
    [SerializeField] public TextMeshProUGUI upgradeText;
    public float increase = 2;
    public int roomIndex;
    public string roomId;
    public bool isUpgraded;
    public bool isLocked;
    private Vector3 increaseSize;

    private void Start()
    {
        fillImg.fillAmount = 0;
        increaseSize = new Vector3(increase, increase, increase);
        Data data = SaveSystem.LoadGame();
        if (data != null)
        {
            foreach (RoomSpawnerData spawnerData in data.allRoomSpawnerData)
            {
                foreach (RoomData roomData in spawnerData.roomsData)
                {
                    isLocked = roomData.isUnLocked;
                    if (!isLocked)
                    {
                        lockCanvas.SetActive(false);
                        this.gameObject.SetActive(false);
                        upgradeText.text = "UPGRADE";
                    }
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if(lockCanvas.activeInHierarchy)
        {
            upgradeText.text = "UNLOCK";
        }
        if(this.isUpgraded)
        {
            foreach (Transform t in room.transform)
            {
                t.localScale = increaseSize;
            }
        }
    }

    public void RoomUnlock(float value)
    {
        fillImg.fillAmount += value;
        if(fillImg.fillAmount >= 1)
        {
            isLocked = false;
            GameManager.instance.SpendCoins(10);
            upgradeText.text = "UPGRADE";
            lockCanvas.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    public void RoomUpgrade(float value)
    {
        fillImg.fillAmount += value;
        if(fillImg.fillAmount >= 1)
        {
            GameManager.instance.SpendCoins(30);
            foreach(Transform t in room.transform)
            {
                t.localScale = increaseSize;
            }
            isUpgraded = true;

            Player system = FindObjectOfType<Player>();
            system.npc_count += 2;
        }
    }
}
