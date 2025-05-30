using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RT_PlayerActions : MonoBehaviour
{
    //Stats
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float delayToBattleArena = 1f;
    [HideInInspector] public bool playerAdvantage;

    //Components
    [HideInInspector] public SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isGrounded;
    private TB_BattleManager battleManager;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    public List<GameObject> team;
    [HideInInspector] public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        battleManager = GetComponent<TB_BattleManager>();
        team.Add(gameObject);
        UI_Manager.Instance.SetSkillButtonsActive(battleManager.currentGameState == TB_BattleManager.GameStates.TurnBased);
        animator.SetTrigger("RT_State");
    }

    void Update()
    {

        if (battleManager.currentGameState == TB_BattleManager.GameStates.RealTime)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (battleManager.currentGameState == TB_BattleManager.GameStates.RealTime)
        {
            Movement();
        }
    }

    void Movement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput < 0f)
        {
            sprite.flipX = true;
        }
        else if (moveInput > 0f)
        {
            sprite.flipX = false;
        }
        animator.SetBool("IsRunning", Mathf.Abs(rb.linearVelocity.x) > 0.01f);
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded)
        {
            Collider2D hitenemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
            animator.SetTrigger("IsAttacking");
            if (hitenemy != null && hitenemy != gameObject)
            {
                StartCoroutine(DelayedTeleportToBattleArea(delayToBattleArena, hitenemy));
            }
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
            animator.SetBool("IsFalling", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsFalling", true);
        }
    }

    IEnumerator DelayedTeleportToBattleArea(float Seconds, Collider2D hitenemy)
    {
        animator.SetTrigger("TB_State");
        yield return new WaitForSeconds(Seconds);
        playerAdvantage = true;
        battleManager.StartBattle(playerAdvantage, team, hitenemy.GetComponent<Enemy>().team);
        Destroy(hitenemy);
        CameraController.Instance.MoveToBattle(transform);
        UI_Manager.Instance.SetSkillButtonsActive(battleManager.currentGameState == TB_BattleManager.GameStates.TurnBased);
    }
}
