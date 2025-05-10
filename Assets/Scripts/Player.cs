using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Collider2D[] results;
    private Vector2 direction;
    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    private bool grounded;
    private bool climbing = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;
        results = Physics2D.OverlapBoxAll(transform.position, size, 0f);
        foreach(Collider2D result in results)
        {
            GameObject hit = result.gameObject;
            if(hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, result, !grounded);
            }
            else if(hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void Update()
    {
        CheckCollision();
        
        if(climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if(grounded && Input.GetButtonDown("Jump"))
        {
            direction.y = jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        
        if(grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }
        if(direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if(climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if(direction.x != 0)
        {
            spriteIndex++;
            if(spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindFirstObjectByType<GameManager>().LevelComplete();
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindFirstObjectByType<GameManager>().LevelFailed();
        }
    }
    
}
