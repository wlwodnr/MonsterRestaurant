using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float detectionRadius = 2f;  //타일 감지 거리

    public GameObject TargetTileObject { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        MoveCharacter();
        FindClosestTileWithRaycast();
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
    
    private void FindClosestTileWithRaycast()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        GameObject closest = null;
        float closestDistance = detectionRadius;

        Vector3 currentPos = this.transform.position;

        foreach (Collider2D col in hitColliders)
        {
            Vector2 direction = (col.transform.position - currentPos).normalized;
            float distance = Vector2.Distance(currentPos, col.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, distance);
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.CompareTag("Plowable"))
            {
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = hit.collider.gameObject;
                }
            }
        }
        TargetTileObject = closest;
    }

    private void PlayerPlowing()
    {
        if(TargetTileObject == null)
        {
            Debug.Log("근처에 개간 가능한 땅이 없습니다.");
            return;
        }
        else
        {
            anim.SetTrigger("Plowing");
            TileManager.Instance.RequestedPlowing(TargetTileObject);
        }
    }
    private void PlayerWatering()
    {
        if(TargetTileObject == null)
        {
            Debug.Log("근처에 물을 줄 수 있는 땅이 없습니다.");
            return;
        }
        else
        {
            anim.SetTrigger("Watering");
            TileManager.Instance.RequestedWatering(TargetTileObject);
        }
    }
    private void OnDrawGizmosSelected() //레이케스트 범위 확인용
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
