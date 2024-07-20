using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public GameObject[] room;
    [SerializeField] float detectionRadius;
    Animator anim;
    bool isBooked = false;

    public int ownerId;

    private Transform queuePosition;
    private float returnTime = 5f;

    private NavMeshAgent navMeshAgent;

    [SerializeField] private Room bookedRoom = null;
    private void Awake()
    {
        anim = GetComponent<Animator>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        QueueManager queueManager = FindObjectOfType<QueueManager>();
        queueManager.EnqueueNPC(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        room = GameObject.FindGameObjectsWithTag("Room");
        Booked();
        UpdateWalkingAnimation();
    }

    private void MoveTowards(Transform target)
    {
        navMeshAgent.SetDestination(target.position);

    }
    private void UpdateWalkingAnimation()
    {
        if (navMeshAgent.velocity.magnitude > 0.5f)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    private void Booked()
    {
        DetectCollision();
        if (isBooked)
        {
            if (bookedRoom != null)
            {
                MoveTowards(bookedRoom.transform);
                if (Vector3.Distance(transform.position, bookedRoom.transform.position) <= 2f)
                {
                    Invoke("ReturnHome", returnTime);
                }
                return;
            }

            foreach (GameObject r in room)
            {
                Room roomScript = r.GetComponent<Room>();


                if (roomScript.CanMoveToRoom() && roomScript.BookRoom(this))
                {
                    GameManager.instance.BuyCoins(20);
                    QueueManager queueManager = FindObjectOfType<QueueManager>();
                    queueManager.DequeueNPC(this);
                    bookedRoom = roomScript;
                    MoveTowards(r.transform);
                    return;
                }
            }
        }
        else
        {
            MoveTowards(queuePosition);
        }
    }
 
    private void ReturnHome()
    {
        isBooked = false;

        if(bookedRoom == null)
        {
            return;
        }
        bookedRoom.MessUpRoom();

        foreach(GameObject r in room)
        {
            Room rooms = r.GetComponent<Room>();
            if (rooms.bookedBy == this)
            {
                rooms.ClearBooking();
            }
        }
        QueueManager queueManager = FindObjectOfType<QueueManager>();
        bookedRoom = null;
        queueManager.EnqueueNPC(this);
        MoveTowards(queuePosition);
    }


    public void SetQueuePosition(Transform position)
    {
        queuePosition = position;
    }
    private void DetectCollision()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in detectedColliders)
        {
            if (collider.TryGetComponent(out CleaningSystem player))
            {
                foreach(GameObject r in room)
                {
                    Room room = r.GetComponent<Room>();
                    if (!room.IsBooked())
                    {
                        if (room.CanMoveToRoom())
                        {
                            isBooked = true;
                            speed = 4f;
                        }
                    }
                    
                    
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
