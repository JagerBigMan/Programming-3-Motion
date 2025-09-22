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
    public float acceleration;
    public Vector3 velocity;


    void Update()
    {
        PlayerMovement();

    }

    private void PlayerMovement()
    {
        //velocity = Vector3.zero; no longer needed cuz I need the speed to add to it, achieving "accelerating"
        acceleration = maxSpeed / accelerationTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.position += new Vector3(-1, 0, 0);  done in class, tried this way to move the ship
            velocity += acceleration * Time.deltaTime * Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.position += new Vector3(1, 0, 0);
            velocity += acceleration * Time.deltaTime * Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.position += new Vector3(0, 1, 0);
            velocity += acceleration * Time.deltaTime * Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.position += new Vector3(0, -1, 0);
            velocity += acceleration * Time.deltaTime * Vector3.down;
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);      //Limits the max speed by clamping
        transform.position += Time.deltaTime * velocity;
    }
}