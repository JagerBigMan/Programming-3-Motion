using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    [Header ("Movement Properties")]
    public float accelerationTime = 3f;
    public float maxSpeed = 5f;
    public float deceleration = 4f;
    public float acceleration;
    public Vector3 velocity;


    void Update()
    {
        PlayerMovement();

    }

    private void PlayerMovement()
    {
        acceleration = maxSpeed / accelerationTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += acceleration * Time.deltaTime * Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += acceleration * Time.deltaTime * Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += acceleration * Time.deltaTime * Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += acceleration * Time.deltaTime * Vector3.down;
        }
        if(!Input.GetKey(KeyCode.LeftArrow) &&
           !Input.GetKey(KeyCode.RightArrow) &&
           !Input.GetKey(KeyCode.UpArrow) &&
           !Input.GetKey (KeyCode.DownArrow))
        {
            velocity = Vector3.MoveTowards(velocity,Vector3.zero, deceleration * Time.deltaTime);
        }


        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);      //Limits the max speed by clamping
        transform.position += Time.deltaTime * velocity;
    }
}