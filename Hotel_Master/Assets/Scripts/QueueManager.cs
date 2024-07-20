using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePositions;
    private Queue<NPC> npcQueue = new Queue<NPC>();
    public float queueSpacing = 2.0f; // Distance between NPCs in the queue

    public void EnqueueNPC(NPC npc)
    {
        npcQueue.Enqueue(npc);
        UpdateNPCPosition();
    }
    public void DequeueNPC(NPC npc)
    {
        if(npcQueue.Count > 0)
        {
            npcQueue.Dequeue();
            npc.room = GameObject.FindGameObjectsWithTag("Room");
            UpdateNPCPosition();
        }
    }
    private void UpdateNPCPosition()
    {
        int i = 0;
        foreach (NPC npc in npcQueue)
        {
            Transform targetPosition = queuePositions[i];
            npc.SetQueuePosition(targetPosition);
            i++;
        }
    }
}
