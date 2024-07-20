using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningSystem : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    [SerializeField] float cleanSpeed;

    // Update is called once per frame
    void Update()
    {
        DetectCleanables();
    }

    private void DetectCleanables()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach(Collider collider in detectedColliders)
        {
            if(collider.TryGetComponent(out Cleanable cleanable))
            {
                if (!cleanable.IsClean())
                {
                    cleanable.Clean(cleanSpeed * Time.deltaTime);
                }
                
            }

            if(collider.TryGetComponent(out UpgradeRoom room))
            {
                if(room.isLocked)
                {
                    room.RoomUnlock(0.4f * Time.deltaTime);
                }
                else
                {
                    room.RoomUpgrade(0.2f * Time.deltaTime);
                }
            }

            if(collider.TryGetComponent(out CounterDesk desk))
            {
                if (desk.hireCanvas.activeInHierarchy)
                {
                    desk.HirePeople(0.2f * Time.deltaTime);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
