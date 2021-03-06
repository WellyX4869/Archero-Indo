using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance // singleton
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }

    [SerializeField] float moveSpeed = 25f;
    FloatingJoystick joyStick;

    public PlayerState playerState;
    private static PlayerMovement instance;
    Rigidbody rb;
    public Animator anim;
    private bool isStopped = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        joyStick = FindObjectOfType<FloatingJoystick>();
        playerState = PlayerState.idle;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = joyStick.Horizontal;
        float moveVertical = joyStick.Vertical;

        if((moveHorizontal != 0 || moveVertical != 0) && !isStopped)
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
            rb.rotation = Quaternion.LookRotation(new Vector3(joyStick.Direction.x * moveSpeed, rb.velocity.y, joyStick.Direction.y * moveSpeed));
            playerState = PlayerState.walk;
            anim.SetTrigger("WALK");
        }
        else
        {
            playerState = PlayerState.idle;
            anim.SetTrigger("IDLE");
        }
    }

    public void StopPlayer()
    {
        isStopped = true;
    }

    public void MovePlayer()
    {
        isStopped = false;
    }
}

public enum PlayerState
{
   idle,
   walk,
   attack
}
