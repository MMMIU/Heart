using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Control enabled
    public bool controlEnabled { get; set; } = true;

    [Space]
    [SerializeField]
    private float moveSpeed = 1.0f;

    [Space]
    [SerializeField]
    private float collisionOffset = 0.1f;
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private LayerMask groundLayer;

    [Space]
    [SerializeField]
    private float jumpForce = 1.0f;
    [SerializeField]
    private bool isJumping = false;
    private bool jumpPressed;
    [SerializeField]
    private int maxJumpCount = 2;
    private int jumpCount = 0;

    [Space]
    private bool isAttacking = false;

    [Space]
    public int HP = 10;
    public void AddHP(double num)
    {
        HP += (int)num;
        Debug.Log("Current HP: " + HP);
    }

    private string playerName;

    private float movementInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!controlEnabled)
        {
            movementInput = 0.0f;
            return;
        }

        // update movements
        movementInput = Input.GetAxisRaw("Horizontal");

        // update jump
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        // update attack
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isAttacking = false;
        }


    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, collisionOffset, groundLayer);
        GroundMovement();
        Jump();
        SwitchAnim();
    }

    public void GroundMovement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
        if (movementInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput < 0)
        {
            spriteRenderer.flipX = true;
        }

    }

    public void Jump()
    {
        if (isAttacking)
        {
            return;
        }

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
            isJumping = false;
        }
        if (jumpPressed && isGrounded)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            --jumpCount;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            --jumpCount;
            jumpPressed = false;
        }
    }

    public void SwitchAnim()
    {
        anim.SetFloat("Speed", Mathf.Abs(movementInput));
        
        if (isJumping)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }
        
        if (isAttacking)
        {
            anim.SetTrigger("IsAttacking");
        }
    }

}
