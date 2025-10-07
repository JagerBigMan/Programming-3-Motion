using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    [Header ("Movement Properties")]
    public float accelerationTime = 3f;
    public float baseMaxSpeed = 5f;     //Non-boosting max speed
    public float deceleration = 4f;
    public float acceleration;
    public Vector3 velocity;

    [Header("Powerups")]
    public GameObject powerupPrefab;

    [Header("Speed Booster")]
    public float boostedMaxSpeed = 10f;
    public float boostRampUp = 6f;      //The acceleration of the  boost, changed the name to avoid confusion
    public float boostRampDown = 4f;    //The deceleration of the  boost

    [Header("Boost Energy")]
    public float maxEnergy = 5f;
    public float energyDrainRate = 1.0f;
    public float energyRechargeRate = 0.75f;
    public Slider energyBar;

    private float currentMaxSpeed;
    private float currentEnergy;

    private void Start()
    {
        currentMaxSpeed = baseMaxSpeed;
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        HandleBoost();
        PlayerMovement();
        DrawDetectionCircle(5f, 64);
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPowerups(6, 4f); //6 powerups at radius 4 units
        }

    }

    private void PlayerMovement()
    {
        acceleration = currentMaxSpeed / accelerationTime;

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


        velocity = Vector3.ClampMagnitude(velocity, currentMaxSpeed);      //Limits the max speed by clamping
        transform.position += Time.deltaTime * velocity;
    }

    private void DrawDetectionCircle(float radius, int sides)
    {
        if (enemyTransform == null || sides < 3) return;

        bool enemyInside = (enemyTransform.position - transform.position).sqrMagnitude < radius * radius;

        Color circleColor = enemyInside ? Color.red : Color.green;

        Vector3 center = transform.position;

        float angleStep = 2f * Mathf.PI / sides;

        Vector3 previousPoint = center + new Vector3(Mathf.Cos(0f), Mathf.Sin(0f), 0f) * radius;

        for (int i = 1; i <= sides; i++)
        {
            float angle = i * angleStep;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 nextPoint = center + new Vector3(x, y, 0f);

            Debug.DrawLine(previousPoint, nextPoint, circleColor);

            previousPoint = nextPoint;

        }
    }

    private void SpawnPowerups(int numberofPowerups, float radius)
    {
        if (powerupPrefab == null || numberofPowerups <= 0) return; 

        float angleStep = 2f * Mathf.PI / numberofPowerups;

        for (int i = 0; i < numberofPowerups; i++)
        {
            float angle = i * angleStep;

            float x = Mathf.Cos(angle) * radius;    
            float y = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = transform.position + new Vector3(x, y, 0f);
            Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void HandleBoost()
    {
        bool boost = Input.GetKey(KeyCode.Space) && currentEnergy > 0f;

        float targetCap = boost ? boostedMaxSpeed : baseMaxSpeed;
        float ramp = boost ? boostRampUp : boostRampDown;
        currentMaxSpeed = Mathf.MoveTowards(currentMaxSpeed, targetCap, ramp * Time.deltaTime);

        if (boost)
        {
            currentEnergy -= energyDrainRate * Time.deltaTime;
            if (currentEnergy < 0f) currentEnergy = 0f;
        }
        else
        {
            currentEnergy += energyRechargeRate * Time.deltaTime;
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        }

    }
}