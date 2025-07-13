using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jump;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckRadius; // bisa aja diassign ke 0.2f 
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AnimatorOverrideController overrideController;
    private bool isGrounded;
    private Animator _animator;
    private SpriteRenderer _sr;
    public bool isDashing;
    float dashTimeLeft;
    int dashDirection;
    public float moveInput;

    void Start()
    {
        isDashing = false;

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = overrideController;
        
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Checker untuk apakah player udah nyentuh platform
        bool isGroundedPrev = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isGroundedPrev && isGrounded)
        {
            AudioManager.instance.PlaySFX("Landing");
        }
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0) && moveInput == -1)
        {
            isDashing = false;
            Debug.Log("Dashing left");
            isDashing = true;
            dashDirection = -1;
            dashTimeLeft = dashDuration;
        }
        else if (Input.GetMouseButtonDown(0) && moveInput == 1)
        {
            isDashing = false;
            Debug.Log("Dashing right");
            isDashing = true;
            dashDirection = 1;
            dashTimeLeft = dashDuration;
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft > 0f)
            {
                rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
                return;
            }
            isDashing = false;
        }

        // Player movement
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            _animator.SetBool("isRunning", true);
            _sr.flipX = true;
        }else if (moveInput < 0)
        {
            _animator.SetBool("isRunning", true);
            _sr.flipX = false;
        }else if (moveInput == 0)
        {
            _animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            AudioManager.instance.PlaySFX("Jump");
        }

        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);
        }else
        {
            _animator.SetBool("isJumping", true);
        }
    }

    public bool isMoving()
    {
        return moveInput != 0;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
