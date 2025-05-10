using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;
    public float speed = 1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Bounds bounds = collider.bounds;
        Vector3 min = Camera.main.WorldToViewportPoint(bounds.min);
        Vector3 max = Camera.main.WorldToViewportPoint(bounds.max);
        if( (max.x < 0) || (min.x > 1) || (max.y < 0) || (min.y > 1))
        {
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }
        
    }
}
