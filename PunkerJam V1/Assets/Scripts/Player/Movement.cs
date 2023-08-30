using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private BoxCollider2D collide;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask jumpableWall;

    // private bool isFacingRight = true;
    
    // Horizontal Movement Speed
    private float acceleration;
    private float multiplierX;
    private float dirX;
    private float varX;
    public float speed;
    private float facing;

    // Vertical Movement
    [SerializeField] private float coyoteTime;
    private float jumpTimeCounter;
    public float jumpForce;
    private float tempValoX;
    // public float maxFallSpeed;

    // On da Wall :) 
    private bool isWallSliding;
    private float WallSlidingSpeed = 1f;

    private bool wallJump;
    private int wallJumpCounter = 0;
    public int maxWallJump;
    private float wallJumpTime;


    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        collide = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Movement
        acceleration = 0;
        multiplierX = 0;
        dirX = Input.GetAxis("Horizontal");
        varX = Input.GetAxisRaw("Horizontal");

        //Kalo di pencet kanan kiri speed max
        if(varX != 0)
        {
            facing = varX;
            multiplierX = varX;
        } else if (dirX !=0 && varX == 0) //Kalau horizontal move key baru dilepas bakal tetep ada akselerasi
        {
            acceleration = (dirX / 4) * speed;
        }
        
        // End of Horizontal Movement

        //Vertical Movement
        if(IsGrounded())
        {
            jumpTimeCounter = coyoteTime;
        } else
        {
            jumpTimeCounter -= Time.deltaTime;
        }

        // Jump 
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && jumpTimeCounter > 0f)
        { 
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            jumpTimeCounter = 0;
        }

        // Fall Speed
        // if (rb.velocity.y < maxFallSpeed)  
        //     // Debug.Log(rb.velocity.y); 
        //     rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

        // Wall Slide
        WallSlide();

        // Wall Jump
        if(Input.GetKeyDown(KeyCode.Space) && wallJumpTime > 0 && wallJumpCounter < maxWallJump)
        {
            wallJump = true; //Aktivasi walljump
            wallJumpTime = 0;
            Invoke(nameof(WallJumpOff), 0.05f); //Mematikan wall jump dalam .05 detik
        }

        if(wallJump)
        {
            float temp = jumpForce;
            if (jumpForce >  25) jumpForce -= 5;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //mengubah velocity
            jumpForce = temp;
        }

        if(IsGrounded())
        {
            wallJumpCounter = 0;
        }

        // Debug.Log(wallJumpTime);
        // End of Jump
    }

    private void FixedUpdate() {

        rb.velocity = new Vector2(multiplierX * speed + acceleration, rb.velocity.y); //rubah velocity
        tempValoX = rb.velocity.x;
        
    }

    // Collider Detection
    private bool IsGrounded()
    {

        return Physics2D.BoxCast(collide.bounds.center, collide.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);    }

    private bool OnTheWall()
    {
        return Physics2D.BoxCast(collide.bounds.center, new Vector2(collide.size.x+.2f, collide.size.y-.5f), 0f, new Vector2(0, 0), 0.1f, jumpableWall);
    }
    // End Collider Detection

    // Wall Movement
    private void WallSlide()
    {
        if(OnTheWall() && !IsGrounded() && Input.GetAxisRaw("Horizontal") != 0f){
            isWallSliding = true;
            wallJumpTime = .08f;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlidingSpeed, float.MaxValue));
        }else {
            isWallSliding = false;
        }
    }

    private void WallJumpOff()
    {
        wallJump = false;
        wallJumpCounter++;
    }

    // End Wall Movement



}
