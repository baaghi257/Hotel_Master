using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public VariableJoystick joystick;
    public float speed;

    public static Player instance;

    public int npc_count;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var moveDir = new Vector3(-joystick.Direction.y, 0, joystick.Direction.x);
        //controller.SimpleMove(moveDir * speed);
        rb.velocity = moveDir * speed;
        if (moveDir.sqrMagnitude <= 0)
        {

            anim.SetBool("Walk", false);
            return;
        }
        else
        {
            anim.SetBool("Walk", true);
        }
        // Rotate the player to face the movement direction
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
        }

    }

}
