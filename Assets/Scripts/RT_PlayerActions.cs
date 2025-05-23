using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Stats
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float attackRange = 0.5f;

    //Components
    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }


    void Movement()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Attack()
    {
        Collider2D hitenemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
        if (hitenemy != null)
        {
            Debug.Log("Hit:" + hitenemy.name);
        }
        else{ Debug.Log("missed"); }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
