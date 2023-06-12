using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarMod : MonoBehaviour
{
    /* 
     * Script used for modifications on the A* Pathfinding project algorithm.
     * This modification allows for the enemy AI to both wander randomly and follow the player.
     */

    public AIPath aiPath;
    float minDist = 15;
    float maxDist = 15;
    int index;

    private Transform human;
    private GameObject humanObj;
    public Transform homePoint;
    public Transform[] AllPoints;
    public PlayerScript Player;

    bool firstCall = true;

    private Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        human = GameObject.FindWithTag("Player").transform; //Get the players Transformation.
        humanObj = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        GameObject[] target = GameObject.FindGameObjectsWithTag("Points");
        AllPoints = new Transform[target.Length]; 
        

        for (int i = 0; i < target.Length; i++) //Get each points transformation from GameObject.
        {
            AllPoints[i] = target[i].transform;
        }
        animator.SetBool("attack", false);

    }
    void Update()
    {
        if(!humanObj.GetComponent<PlayerScript>().isRotated)
        {
            // Changes where the enemy AI is looking (east/west) depending on where they are going.
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else
        {
            Debug.Log("Rotated");
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        // Calculate distance between player and transform.
        float dist = Vector2.Distance(human.position, transform.position);
        //float dist = aiPath.remainingDistance;

        // If the player is close enough to Ai, target the player.
        if (dist < minDist)
        {
            this.aiPath.target = human;

            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic; //Change to Dynamic to avoid enemies stacking when following the player

            firstCall = true;
            animator.SetBool("attack", true);
        }

        // If the player is far enough from the AI, start roaming.
        if (dist > maxDist)
        {
            followPath();

            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; //Change to Kinematic to avoid allow ememies to pass through each other when pathfinding.
            animator.SetBool("attack", false);
        }


    }

    // A flag is used to first make the AI go to its designated 'home point'.
    // After a random point from all points found in the map is randomly picked everytime the ai reaches the end of its current path.
    public void followPath()
    {
        if (firstCall)
        {
            this.aiPath.target = homePoint;
            firstCall = false;
        }
        else
        {
            if (this.aiPath.reachedEndOfPath)
            {
                this.aiPath.target = pickRandomTarget();
            }
        }
    }


    public Transform pickRandomTarget()
    {
        Transform nextTarget;
        index = Random.Range(0, AllPoints.Length);
        nextTarget = AllPoints[index];

        return nextTarget;

    }
}
