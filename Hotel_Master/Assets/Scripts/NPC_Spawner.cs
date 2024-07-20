using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    [SerializeField] GameObject NPC_Prefab;
    [SerializeField] QueueManager queueManager;

    public int npcCount;
    public static NPC_Spawner instance;

    private Coroutine spawnCoroutine;
    private int totalNPCsCount = 4;

    private int currentOwnerId;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public float spawnInterval = 10.0f; // Time interval between spawns

    private void Start()
    {
        npcCount = FindObjectsOfType<NPC>().Length;
        StartSpawninig();
    }

    private void StartSpawninig()
    {
        if(spawnCoroutine == null && npcCount < queueManager.queuePositions.Length)
        {
            StartCoroutine(SpawnNPCs());
        }
    }
    private IEnumerator SpawnNPCs()
    {
        while (npcCount < queueManager.queuePositions.Length)
        {
            GameObject npcObj = Instantiate(NPC_Prefab, transform.position, Quaternion.identity);
            npcObj.name = NPC_Prefab.name + npcCount;
            NPC npc = npcObj.GetComponent<NPC>();
            npc.ownerId = currentOwnerId++;
            npcCount++;
            yield return new WaitForSeconds(spawnInterval);

        }
    }

/*    public void NotifyNPCDestroyed(int ownerId)
    {
        npcCount--;
        npcCount = FindObjectsOfType<NPC>().Length;
        if (npcCount < queueManager.queuePositions.Length)
        {
            StartSpawninig();
        }
    }*/
}
