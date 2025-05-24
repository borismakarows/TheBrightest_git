using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RT_PlayerActions : MonoBehaviour
{
    //Stats
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float attackRange = 0.5f;
    public bool playerAdvantage;

    //Components
    private Rigidbody2D rb;
    private bool isGrounded;
    private TB_BattleManager battleManager;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    public List<GameObject> team;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        battleManager = GetComponent<TB_BattleManager>();
        team.Add(gameObject);
    }

    void Update()
    {
        if (battleManager.currentGameState == TB_BattleManager.GameStates.RealTime)
        {
            Movement();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider2D hitenemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
            if (hitenemy != null)
            {
                Debug.Log("Hit:" + hitenemy.name);
                playerAdvantage = true;
                battleManager.StartBattle(playerAdvantage, team, hitenemy.GetComponent<Enemy>().team);
            }
            else { Debug.Log("missed"); }
        }
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
