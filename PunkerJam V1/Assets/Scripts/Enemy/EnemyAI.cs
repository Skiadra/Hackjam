using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAi : MonoBehaviour
{
    [Header("Pathfinding")]
    private GameObject player;
    public float activationDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool IsGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;

    private void Start() 
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate() 
    {
        if(TargetInDistance() && followEnabled)
        {
            PathFollow();
        }

        Physics2D.IgnoreLayerCollision(3,8);
    }

    private void UpdatePath()
    {
        if(TargetInDistance() && followEnabled && seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
        }
    }

    private void PathFollow(){
        if(path == null)
        {
            return;
        }

        // Sampai ditujuan
        if(currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Collider Check
        IsGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        // Arah
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Jump 
        if(jumpEnabled && IsGrounded)
        {
            if(direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        // Move
        rb.AddForce(force);

        // Arah Selanjutnya
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Arah Pandang Sprite
        if(directionLookEnabled)
        {
            if(rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);  
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    } 

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, player.transform.position) < activationDistance;
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
