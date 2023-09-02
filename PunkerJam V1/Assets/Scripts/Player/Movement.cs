using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement move;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D collide;
    private Animator anima;
    private Transform aimTransform;


    // [SerializeField] private TrailRenderer tr;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;

    //SaveLoadSystem
    [SerializeField] private bool saveLoadSystem;

    [Header("Basic Movement")]

    public float speed;
    public bool inControl;
    private float multiplierX;
    private float dirX;
    private float varX;
    public float facing;
    private float acceleration;
    public float jumpForce;
    [SerializeField] private float coyoteTime;
    private float jumpTimeCounter;

    // [Header("Dashing")]

    // //Dashing Var
    // [SerializeField] private bool canDash;
    // private bool isDashing = false;
    // private float dashingTime = 0.15f;
    // private float dashingCooldown = 1f;
    // private bool dashReset;
    // public bool canDashReset = false;
    // [SerializeField] private float dashingPower;


    //WallMovement
    [Header("Wall Movement")]
    private bool isWallSliding;
    private float WallSlidingSpeed = 1f;

    private bool wallJump;
    private int wallJumpCounter = 0;
    public int maxWallJump;
    private float wallJumpTime;
    // public float xWallJump;
    


    [Header("Melee")]

    //Attack Combat
    public int attack; //Damage
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    [SerializeField] ParticleSystem slash;
    [SerializeField] ParticleSystem chargeSlash;
    public float nextAttackRate = 2f;
    float nextAttackTime = 0f;
    public float currentFacingTime = 1;
    //Charged Attack
    public float chargeSpeed = 2f;
    public int upSlash = 0;
    float chargeTime;
    public float chargeRange = 10f;
    private bool isCharging;


    private enum Status { idle, walking, running, jumping, falling, wallSliding }
    private Status state = Status.idle;
    private void Awake()
    {
        if (move != null & move != this)
        {
            Destroy(this);
        }
        else
        {
            move = this;
        }

        aimTransform = transform.Find("Aim");
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (saveLoadSystem)
        {
            if (GameManager.loadStat)
            {
                loadPos();
                GameManager.loadStat = false;
            }
            if (!GameManager.newStat)
            {
                loadData();
                Debug.Log("Loaded");
            }
            GameManager.newStat = false;
            // skillTree.skillPointAdd();
            saveData();
        }
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();
        anima = GetComponent<Animator>(); 
        inControl = true;
        facing = -1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!inControl)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);//Kalo lagi buka menu ga bisa gerak
            return;
        }

        //ATTACK SEGMENT =====================================================================================================
        //Check if arrow up is pressed
        if (Input.GetKey(KeyCode.UpArrow)) upSlash = 1;
        else upSlash = 0;
        currentFacingTime = facing;

        //Charge Attack
        if (Input.GetKey(KeyCode.X) && chargeTime < 2)
        {
            isCharging = true;
            if (isCharging)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
            }
        }

        if (Input.GetKeyUp(KeyCode.X) && chargeTime >= 2)
        {
            ChargeAttack();
            chargeSlash.Play();
            isCharging = false;
        }
        else if (Input.GetKeyUp(KeyCode.X) && chargeTime < 2)
        {
            chargeTime = 0f;
            isCharging = false;

            //Normal Attack
            if (Time.time >= nextAttackTime)
            {
                if (!isWallSliding)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / nextAttackRate;
                    slash.Play();
                }
            }
        }

        //Normal Attack
        // if (Time.time >= nextAttackTime && !isCharging)
        // {
        //     if (Input.GetKeyDown(KeyCode.C) && !wallHug)
        //     {
        //         Attack();
        //         nextAttackTime = Time.time + 1f / nextAttackRate;
        //         slash.Play();
        //     }
        // }

        //HORIZONTAL MOVEMENT ===========================================================================================================
        acceleration = 0;
        multiplierX = 0;
        dirX = Input.GetAxis("Horizontal");
        varX = Input.GetAxisRaw("Horizontal");

        if (varX != 0)
        {
            facing = varX;
            multiplierX = varX;
        }
        else if (dirX != 0 && varX == 0)
        {
            acceleration = dirX / 4 * speed;
        }

        //VERTICAL MOVEMENT========================================================================================================
        if (IsGrounded())
        {
            jumpTimeCounter = coyoteTime;
        }
        else
        {
            jumpTimeCounter -= Time.deltaTime;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() &&  jumpTimeCounter > 0f && !isCharging)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            jumpTimeCounter = 0;
        }

        // Wall Slide
        WallSlide();

        //Wall Jump

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpTime > 0 && wallJumpCounter < maxWallJump && !isCharging)
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

        Animate();

        //Ignore Collisions
        // Physics2D.IgnoreLayerCollision(3,8);
    }

    private void FixedUpdate() {
        if (!isCharging)
            rb.velocity = new Vector2(multiplierX * speed + acceleration, rb.velocity.y); //rubah velocity
    }
    private void Animate()
    {
        Status state;

        if (dirX > 0 && !isWallSliding)
        {
            sprite.flipX = false;
            state = Status.walking;
        }
        else if (dirX < 0 && !isWallSliding)
        {
            state = Status.walking;
            sprite.flipX = true;
        }
        else
        {
            state = Status.idle;
        }

        if (rb.velocity.y > 0.01 && !isWallSliding)
        {
            state = Status.jumping;
        }
        else if (rb.velocity.y < -0.1 && IsGrounded() == false && !isWallSliding)
        {
            state = Status.falling;
        }

        if (wallJump)
        {
            sprite.flipX = true;
        }

        if (isWallSliding)
        {
            state = Status.wallSliding;
        }

        if (isCharging)
        {
            state = Status.idle;
        }

        anima.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {

        return Physics2D.BoxCast(collide.bounds.center, collide.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool OnTheWall()
    {
        return Physics2D.BoxCast(collide.bounds.center, new Vector2(collide.size.x+.2f, collide.size.y-.5f), 0f, new Vector2(0, 0), 0.1f, jumpableWall);
    }

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

    //Save-Load method
    public void saveData()
    {
        SaveSystem.SavePlayer(this);
    }

    public void loadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        jumpForce = data.jumpForce;
        maxWallJump = data.maxWallJump;
        // skillTree.addSkillPoints = data.skillPointsEachLevel;
        // for (int i = 0; i < skillTree.unlocked.Length; i++)
        // {
        //     skillTree.unlocked[i] = false;
        // }
        // for (int i = 0; i < skillTree.unlocked.Length; i++)
        // {
        //     skillTree.unlocked[i] = data.unlockedSkill[i];
        // }
        // skillTree.skillPoint = data.points;
        // skillTree.UpdateSkillUI();
    }

    public void loadPos()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Vector2 pos;
        pos.x = data.position[0];
        pos.y = data.position[1];

        transform.position = pos;
    }

    // private IEnumerator Dash()
    // {
    //     canDash = false;
    //     isDashing = true;
    //     float originalGravity = rb.gravityScale;
    //     rb.gravityScale = 0; //Gravitasi menjadi 0
    //     rb.velocity = new Vector2(facing * dashingPower, 0); //Mengubah velocity
    //     tr.emitting = true; //Trail nyala
    //     yield return new WaitForSeconds(dashingTime);
    //     dirX = .4f;
    //     tr.emitting = false;
    //     rb.gravityScale = originalGravity;
    //     isDashing = false;
    //     yield return new WaitForSeconds(dashingCooldown);
    //     canDash = true;
    // }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyData>().TakeDamage(attack);
        }
    }

    private void ChargeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(new Vector2(attackPoint.position.x +
        transform.localScale.x * chargeRange / 2 * currentFacingTime * (1 - upSlash), attackPoint.position.y +
        transform.localScale.y * chargeRange / 2 * upSlash),
        new Vector2(transform.localScale.x * chargeRange * (1 - upSlash), 1 + (transform.localScale.y * chargeRange * upSlash)),
        0, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyData>().TakeDamage(attack * 2);
            Debug.Log("Charge Hit");
        }
        chargeTime = 0f;
    }


    void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        // Gizmos.DrawCube(new Vector3(attackPoint.position.x +
        // transform.localScale.x * chargeRange / 2 * currentFacingTime * (1 - upSlash),
        // attackPoint.position.y + transform.localScale.y * chargeRange / 2 * upSlash),
        // new Vector2(transform.localScale.x * chargeRange * (1 - upSlash) + 1, 1 + (transform.localScale.y * chargeRange * upSlash)));
    }
}
