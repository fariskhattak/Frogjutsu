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
    [SerializeField] private LayerMask jumpableGround;
    public float dirX = 0f;

    private enum MovementState { idle, running, jumping, falling }
    private enum SpecialMovementState {idle, running, jumping, falling}
    public bool isSprinting;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Player Movement is now awake!");
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
        // if (Input.GetButtonDown("Fire2") && IsGrounded())
        // {
        //     Debug.Log("Player started sprinting");
        //     isSprinting = true;
        // }
        // if (Input.GetButtonUp("Fire2"))
        // {
        //     Debug.Log("Player stopped sprinting");
        //     isSprinting = false;
        // }
        // if (!IsGrounded())
        //     isSprinting = false;
        // if (!isSprinting)
        //     player.Run(dirX);
        // else
        //     player.Sprint(dirX);

        player.Run(dirX);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.Jump();
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;
        SpecialMovementState specialState;
        // Running right
        if (dirX > 0f)
        {
            state = MovementState.running;
            specialState = SpecialMovementState.running;
            rb.transform.localScale = Vector3.one;
        }
        // Running left
        else if (dirX < 0f)
        {
            state = MovementState.running;
            specialState = SpecialMovementState.running;
            rb.transform.localScale = new Vector3(-1, 1, 1);
        }
        // Stopped
        else
        {
            state = MovementState.idle;
            specialState = SpecialMovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
            specialState = SpecialMovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
            specialState = SpecialMovementState.falling;
        }

        if (player is Warrior && player.specialAbilityActivated)
        {
            anim.SetInteger("special", (int)specialState);
            anim.SetInteger("state", -1);
        }
        else
        {
            anim.SetInteger("state", (int)state);
        }

    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


}
