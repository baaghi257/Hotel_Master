using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Player : MonoBehaviour
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private float cleanSpeed;

    private NavMeshAgent agent;
    [SerializeField]private Cleanable currentTarget;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectAndMoveToMessyRooms();
        UpdateWalkingAnimation();
    }
    private void UpdateWalkingAnimation()
    {
        if (agent.velocity.magnitude > 0.5f)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    private void DetectAndMoveToMessyRooms()
    {
        if (currentTarget == null || currentTarget.IsClean())
        {
            Collider[] detectedCollider = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach(Collider collider in detectedCollider)
            {
                if(collider.TryGetComponent(out Cleanable cleanable))
                {
                    if (!cleanable.IsClean())
                    {
                        currentTarget = cleanable;
                        agent.SetDestination(cleanable.transform.position);
                        return;
                    }
                }
            }
        }
        
        if(currentTarget != null && Mathf.RoundToInt(Vector3.Distance(transform.position, currentTarget.transform.position)) <= agent.stoppingDistance + 1)
        {
            currentTarget.Clean(cleanSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
