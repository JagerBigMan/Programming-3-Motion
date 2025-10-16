using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    [Header("Movement Properties")]
    public float accelerationTime = 3f;
    public float baseMaxSpeed = 5f;     //Non-boosting max speed
    public float deceleration = 4f;
    public float acceleration;
    public Vector3 velocity;
    private bool isMoving;

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

    public bool overheated = false;
    public float overheatCooldown = 3f;
    private float overheatTimer = 0f;

    private float currentMaxSpeed;
    private float currentEnergy;

    [Header("Wingmen")]
    public GameObject wingmenPrefab;
    public float wingmenOrbitRadius = 2f;
    public float wingmenOrbitSpeed = 10f;   //Degrees per second    
    public bool toggleSpawn = true;         //If true, W toggles on/off, if false, W respawns the wingmen pair

    private Transform wingmenPivot;
    private GameObject leftWingman;
    private GameObject rightWingman;

    private void Start()
    {
        currentMaxSpeed = baseMaxSpeed;
        currentEnergy = maxEnergy;

        wingmenPivot = new GameObject("WingmanPivot").transform;
        wingmenPivot.SetParent(transform);
        wingmenPivot.localPosition = Vector3.zero;
        wingmenPivot.localRotation = Quaternion.identity;
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

        HandleWingmenInput();
        RotateWingmen();
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
        if (!Input.GetKey(KeyCode.LeftArrow) &&
           !Input.GetKey(KeyCode.RightArrow) &&
           !Input.GetKey(KeyCode.UpArrow) &&
           !Input.GetKey(KeyCode.DownArrow))
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, deceleration * Time.deltaTime);
        }


        velocity = Vector3.ClampMagnitude(velocity, currentMaxSpeed);      //Limits the max speed by clamping
        transform.position += Time.deltaTime * velocity;

        isMoving = Input.GetKey(KeyCode.LeftArrow) ||
                   Input.GetKey(KeyCode.RightArrow) ||
                   Input.GetKey(KeyCode.UpArrow) ||
                   Input.GetKey(KeyCode.DownArrow);
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
        if (overheated)     //cooling phase
        {
            overheatTimer -= Time.deltaTime;
            if (overheatTimer <= 0f)
            {
                overheated = false;
            }
            return;
        }
        bool boost = Input.GetKey(KeyCode.Space) && currentEnergy > 0f && isMoving;

        float targetCap = boost ? boostedMaxSpeed : baseMaxSpeed;
        float ramp = boost ? boostRampUp : boostRampDown;
        currentMaxSpeed = Mathf.MoveTowards(currentMaxSpeed, targetCap, ramp * Time.deltaTime);

        if (boost)
        {
            currentEnergy -= energyDrainRate * Time.deltaTime;
            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                overheated = true;
                overheatTimer = overheatCooldown;       //Start cooldown timer
            }
        }
        else
        {
            currentEnergy += energyRechargeRate * Time.deltaTime;
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        }
        if(energyBar != null)
        {
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
        }
    }

    private void HandleWingmenInput()
    {
        if (!Input.GetKeyDown(KeyCode.W)) return;

        bool exists = leftWingman != null || rightWingman != null;

        if (toggleSpawn && exists)
        {
            DespawnWingmen();
        }
        else
        {
            SpawnWingmenPair();
        }
    }

    private void SpawnWingmenPair()
    {
        if (wingmenPrefab == null || wingmenPivot == null) return;

        DespawnWingmen();

        leftWingman = Instantiate(wingmenPrefab,wingmenPivot.position + Vector3.left * wingmenOrbitRadius, Quaternion.identity);
        rightWingman = Instantiate(wingmenPrefab,wingmenPivot.position + Vector3.right * wingmenOrbitRadius, Quaternion.identity);

        leftWingman.transform.SetParent(wingmenPivot);
        rightWingman.transform.SetParent(wingmenPivot);
    }

    private void RotateWingmen()
    {
        if (wingmenPivot == null) return;

        wingmenPivot.position = transform.position;     //Always re-center pivot on the player

        float detectionRadius = 5f;
        float defensiveRadius = 1.2f;
        float normalOrbitRadius = wingmenOrbitRadius;
        bool enemyNearby = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < detectionRadius)
            {
                enemyNearby = true;
                break;
            }
        }

        float targetRadius = enemyNearby ? defensiveRadius : normalOrbitRadius;
        float orbitAdjustSpeed = 3f;        //How quickly the orbit radius changes, how fast the wingmen go into defensive position

        wingmenPivot.Rotate(0f,0f, wingmenOrbitSpeed * Time.deltaTime);

        if (leftWingman)
        {
            Vector3 directionToLeft = (leftWingman.transform.localPosition).normalized;
            leftWingman.transform.localPosition = Vector3.Lerp(leftWingman.transform.localPosition, directionToLeft * targetRadius, Time.deltaTime * orbitAdjustSpeed);
            leftWingman.transform.rotation = Quaternion.identity;
        }
        if (rightWingman)
        {
            Vector3 directionToRight = (rightWingman.transform.localPosition).normalized;
            rightWingman.transform.localPosition = Vector3.Lerp(rightWingman.transform.localPosition, directionToRight * targetRadius, Time.deltaTime * orbitAdjustSpeed);
            rightWingman.transform.rotation = Quaternion.identity;
        }
        wingmenPivot.Rotate(0f, 0f, wingmenOrbitSpeed * Time.deltaTime);
    }

    private void DespawnWingmen()
    {
        if (leftWingman) Destroy(leftWingman);
        if (rightWingman) Destroy(rightWingman);
        leftWingman = null;
        rightWingman = null;
    }
}