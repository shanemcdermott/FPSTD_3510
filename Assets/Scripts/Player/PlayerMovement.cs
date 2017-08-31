using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 6f;

    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigidBody;
    private int floorMask;
    private float camRayLength = 100f;
    
    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    //Runs with every physics update
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        UpdateTurn();
        Move(h, v);
        UpdateAnim(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * walkSpeed *Time.deltaTime;
 
        playerRigidBody.MovePosition(transform.position + movement);
    }

    void UpdateTurn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRot = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRot);
        }
    }

    void UpdateAnim(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
