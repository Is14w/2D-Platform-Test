using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region [SerializeField]Varibles
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] float jumpVelocity = 5.0f;
    [SerializeField] float dashSpeed;
    [SerializeField] public static bool isDash;
    [SerializeField] float resumeTime;
    [SerializeField] float dashLastingTime;
    [SerializeField] public static bool canDoubleJump = false;
    [SerializeField] public static float Gravity;
    [SerializeField] public float dashCD = 4.0f;
    [SerializeField] private float dashTimer;
    [SerializeField] public int maxDashCount = 3;
    [SerializeField] private int dashCount;
    [SerializeField] public bool ifHitCollision = false;
    #endregion

    #region otherVaribles
    private bool jumped = false;
    public static float moveBias = 1;
    private Rigidbody2D rb;
    private int dir = 0;
    private Animator animator;
    public GameObject Echo;
    public int echoNum = 5;
    float echoTime;
    #endregion

    #region Awake
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Start
    private void Start()
    {
        jumped = false;
        Gravity = rb.gravityScale;
        dashCount = maxDashCount;
    }
    #endregion

    #region Update
    private void Update()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("Run", playerHasXAxisSpeed);
        IsMove();
        Jump();
        UpdateAnimationState();
        Dash();
    }
    #endregion

    #region Movement
    public void Move(float x)
    {
        rb.velocity = Vector2.Lerp(
            rb.velocity, new Vector2(x, rb.velocity.y), 0.5f);   
    }

    public void IsMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && !isDash)
        {
            float input_X = Input.GetAxis("Horizontal");
            Move(input_X * moveSpeed * moveBias);
            if(!PlayerAttack.isAttacking)
            {
                FaceDirection(input_X);
            }
        }
    }
    private void FaceDirection(float x)
    {
        if (x > 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        this.transform.localScale = new Vector3(dir, 1, 1);
    }
    #endregion

    #region Dash
    public bool CanDash()
    {
        return (Input.GetMouseButton(2) && dashCount > 0 && !isDash);
    }

    private void Dash()
    {
        if (dashCount < maxDashCount && Time.time > dashTimer)
        {
            dashCount++;
            if (dashCount < maxDashCount)
            {
                dashTimer += dashCD;
            }
            else
            {
                dashTimer = float.PositiveInfinity;
            }
        }
        if (CanDash())
        {
            animator.SetBool("Dash", true);
            isDash = true;
            dashCount--;
            dashTimer = Time.time + dashCD;
            resumeTime = Time.time + dashLastingTime;
            rb.velocity = new Vector2(dir * dashSpeed, 0);
            rb.gravityScale = 0;
        }
        if (Time.time > resumeTime && isDash)
        {
            animator.SetBool("Dash", false);
            isDash = false;
            rb.gravityScale = Gravity;
            rb.velocity = new Vector2(dir * moveSpeed, 0);
        }
        if (isDash && Time.time > echoTime)
        {
            if (dir == -1)
            {
                Instantiate(Echo, transform.position, Quaternion.Euler(new Vector3(0, dir * 180, 0)));
            }
            else
            {
                Instantiate(Echo, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            echoTime = Time.time + dashLastingTime / echoNum;
        }
    }
    #endregion

    #region Jump
    private void Jump(float jumpBias = 1.0f)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canDoubleJump && !jumped)
            {
                this.rb.velocity = new Vector2(this.rb.velocity.x, jumpVelocity * jumpBias);
                jumped = true;
            }
            else if (canDoubleJump)
            { 
                this.rb.velocity = new Vector2(this.rb.velocity.x, jumpVelocity * jumpBias);
                canDoubleJump = false;
                jumped = false;
            }
        }
    }
    #endregion

    #region UpdateAnimationState
    private void UpdateAnimationState()
    {
        animator.SetBool("Idle", false);
        if (rb.velocity.y > 0.01f && !CheckOnGround.isOnGround)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Fall", false);
        }
        if (rb.velocity.y < -0.01f && !CheckOnGround.isOnGround)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", true);
        }
        if (CheckOnGround.isOnGround)
        {
            animator.SetBool("Fall", false);
            animator.SetBool("Idle", true);
        }
    }
    #endregion

}
