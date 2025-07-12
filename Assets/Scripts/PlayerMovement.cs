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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AnimatorOverrideController overrideController;
    private bool isGrounded;
    private Animator _animator;
    private SpriteRenderer _sr;
    public float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = overrideController;
        
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Checker untuk apakah player udah nyentuh platform
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Player movement
        moveInput = Input.GetAxisRaw("Horizontal");
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
