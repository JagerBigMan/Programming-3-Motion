using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;

public class Enemy : MonoBehaviour
{
    public Transform player;        //Reference to the player
    public Transform pointA;        //Start of patrol path
    public Transform pointB;        //End of patrol path 
    public float moveSpeed = 3f;    
    public float detectionRange = 6f;

    private Transform currentTarget;    //Where the enemy is moving to

    private void Start()
    {
        currentTarget = pointA;
    }
    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.2f)
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
            }
            else
            {
                currentTarget = pointA;
            }
        }
    }
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
