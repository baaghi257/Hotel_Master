using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterDesk : MonoBehaviour
{
    public static CounterDesk instance;

    public GameObject hireCanvas;
    [SerializeField] Image fillImg;
    [SerializeField] GameObject counterPos;
    [SerializeField] GameObject counterPrefab;
    [SerializeField] GameObject cleaningNPCPrefab;
    [SerializeField] Transform[] cleanNPC_Pos;

    public bool hasPlayerSpawned = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        hireCanvas.SetActive(false);
        fillImg.fillAmount = 0;
    }

    private void Start()
    {
        if(hasPlayerSpawned == true)
        {
            StartCoroutine(SpawnCounterPlayer());
        }
    }
    private void Update()
    {
        Player player = FindObjectOfType<Player>();
        if(player.npc_count == 2)
        {
            hireCanvas.SetActive(true);
        }
    }
    public void HirePeople(float value)
    {
        fillImg.fillAmount += value;
        if(fillImg.fillAmount >= 1 && !hasPlayerSpawned)
        {
            StartCoroutine(SpawnCounterPlayer());
            
        }
        else
        {

            hireCanvas.SetActive(false);
        }
    }

    IEnumerator SpawnCounterPlayer()
    {
        GameObject player = Instantiate(counterPrefab, counterPos.transform.position, Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        foreach(Transform t in cleanNPC_Pos)
        {
            Instantiate(cleaningNPCPrefab, t.position, Quaternion.identity);
        }
        hasPlayerSpawned = true;
        yield return null;
    }
}
