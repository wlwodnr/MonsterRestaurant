using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    [SerializeField]
    private float moveSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        MoveCharacter();

        if (Input.GetKeyDown((KeyCode.Q))) 
        {
            PlayerPlowing();
        }
        if (Input.GetKeyDown((KeyCode.E)))
        {
            PlayerWatering();
        }
        if(Input.GetKeyDown((KeyCode.F)))
        {
            anim.SetTrigger("Harvesting");
        }


    }
    
    private void MoveCharacter()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(xInput, yInput);

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        
        if(xInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(xInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        bool isMoving = moveDirection.magnitude > 0;
        anim.SetBool("IsWalk", isMoving);

    }
    
    private void PlayerPlowing()
    {
        anim.SetTrigger("Plowing");
        TileManager.Instance.RequestedPlowing();
    }

    private void PlayerWatering()
    {
        anim.SetTrigger("Watering");
        TileManager.Instance.RequestedWatering();
    }



   
}
