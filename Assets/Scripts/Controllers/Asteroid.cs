using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 2f;        //How fast asteroids moves
    public float arrivalDistance = 0.5f;        //Distance at which asteroid arrives at target
    public float maxFloatDistance = 5f;         //Max random distance to pick a new target

    private Vector3 targetPoint;

    void Start()
    {
        ChooseNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;      //Move toward the current target
        transform.position += direction * moveSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, transform.position) < arrivalDistance)      //If close enough, pick a new point
        {
            ChooseNewTarget();
        }    
    }
    private void ChooseNewTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;       //Pick a random direction in a circle with a radius of 1.
                                                                            //This is sort of like throwing a dart at a dartboard, the random point can be chosen anywhere inside that circle. 

        Vector3 offset = new Vector3(randomDirection.x, randomDirection.y, 0f) * maxFloatDistance;

        targetPoint = transform.position + offset;
    }
}
