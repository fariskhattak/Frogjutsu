using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX = 0f;

    private enum MovementState { idle, running, jumping, falling }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        if (player is Warrior)
            player = (Warrior)player;
        else if (player is Mage)
            player = (Mage)player;
        else if (player is Assassin)
            player = (Assassin)player;
        else if (player is Ranger)
            player = (Ranger)player;
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        player.Run(dirX);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("isAttacking", true);
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;
        // Running right
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        // Running left
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        // Stopped
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void EndAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    public void Attack()
    {
        if (attackPoint != null)
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

            foreach (Collider2D enemyGameObject in enemy)
            {
                Debug.Log("Hit Enemy");
                enemyGameObject.GetComponent<EnemyHealth>().health -= player.damage;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
        }

    }
}
