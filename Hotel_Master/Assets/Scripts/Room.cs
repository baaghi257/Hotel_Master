using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField] int npc_Count;
    [SerializeField] GameObject upgradeCanvas;
    [SerializeField] UpgradeRoom upgradeRoom;
    [SerializeField] NavMeshSurface navMeshSurface;

    public bool canSpawnNewRoom;

    public NPC bookedBy = null;

    private void OnEnable()
    {
        upgradeRoom.isLocked = true;
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        BakeNavMesh();

    }

    private void BakeNavMesh()
    {
        NavMesh.RemoveAllNavMeshData();

        navMeshSurface.BuildNavMesh();
    }

    void Update()
    {
        if(npc_Count >= 3)
        {
            upgradeCanvas.SetActive(true);
        }
        if (upgradeRoom.isUpgraded == true)
        {
            npc_Count = 0;
            upgradeRoom.isUpgraded = false;
            upgradeRoom.increase += 0.2f;
            upgradeCanvas.SetActive(false);

        }
        if(upgradeRoom.increase > 2f && upgradeRoom.isLocked == false)
        {
            canSpawnNewRoom = true;
            upgradeCanvas.SetActive(false);
        }

    }
    public bool IsBooked()
    {
        return bookedBy != null;
    }

    public bool BookRoom(NPC nPC)
    {
        if (IsBooked()){
            return false;
        }

        npc_Count += 2;
        bookedBy = nPC;
        return true;
    }

    public void ClearBooking()
    {
        bookedBy = null;
    }

    public NPC GetBookedBy()
    {
        return bookedBy;
    }

    public bool CanMoveToRoom()
    {
        Cleanable[] cleanable = GetComponentsInChildren<Cleanable>();
        foreach(Cleanable clean in cleanable)
        {
            if (!clean.IsClean())
            {
                return false;
            }
        }
        return true;
    }
    public void MessUpRoom()
    {
        Cleanable[] cleanables = GetComponentsInChildren<Cleanable>();
        foreach (Cleanable cleanable in cleanables)
        {
            cleanable.MessUp();
        }

    }
}
