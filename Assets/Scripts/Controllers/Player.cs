using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public Vector2 bombOffset = new Vector2(0, 1);      //Single bomb spawning by pressing b

    public float bombTrailSpacing = 1f;     //Distance between the trail bombs
    public int numberOfTrailBombs = 3;      //How many bombs to spawn in the trail

    public float randomCornerDistance = 2f;     //Distance from player to each corner

    public float asteroidLineRange = 8f;        //Max range to check for asteroids

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))                //Spawns a singular bomb at the fixed location
        {
            SpawnBombAtOffset(bombOffset);  //Same as Vector3.up since it's 0,1, got it from Jeff in class
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBombTrail(bombTrailSpacing, numberOfTrailBombs);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnBombAtRandomCorner(randomCornerDistance);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            WarpToTarget(enemyTransform, 1f);
        }

        DrawAsteroidDirectionLines(asteroidTransforms, asteroidLineRange);
    }


    private void SpawnBombAtOffset(Vector2 inOffset)
    {
        Vector3 spawnPos = transform.position + (Vector3)inOffset;
        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }

    private void SpawnBombTrail(float bombspacing, int numberOfBombs)
    {
        for (int i = 1; i <= numberOfBombs; i++)
        {
            Vector3 offset = Vector3.down * (i * bombspacing);      //Behind is relative to the ship's position, so in this case behind would be Vector3.down
            Vector3 spawnPos = transform.position + offset;

            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void SpawnBombAtRandomCorner(float inDistance)
    {
        Vector2 center = transform.position;    //I'm using Vector2 center as the player's location (spaceship)
        Vector2 size = new Vector2(inDistance * 2f, inDistance * 2f);

        float halfWidth = size.x * 0.5f;
        float halfHeight = size.y * 0.5f;

        Vector2 topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
        Vector2 topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
        Vector2 bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
        Vector2 bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);

        Vector2[] corners = { topLeft, topRight, bottomRight, bottomLeft };         //I created an array for the four corners relative to the ship

        int index = Random.Range(0, corners.Length);
        Vector2 chosen = corners[index];

        Instantiate(bombPrefab, chosen, Quaternion.identity);

    }

    private void WarpToTarget(Transform target, float ratio)
    {
        if (target == null) return;

        if (ratio < 0f) ratio = 0f;
        if (ratio > 1f) ratio = 1f;

        transform.position = Vector3.Lerp(transform.position, target.position, ratio);
    }

    private void DrawAsteroidDirectionLines(List<Transform> inAsteroids, float maxRange)
    {
        if (inAsteroids == null || inAsteroids.Count == 0) return;      //Safety check

        Vector3 origin = transform.position;
        const float lineLength = 2.5f;
        float maxRangeSqr = maxRange * maxRange;

        foreach (Transform asteroid in inAsteroids)     //Looping through all the asteroids
        {
            if (!asteroid) continue;        //Skip all the null entries 

            Vector3 toAsteroid = asteroid.position - origin;        //Find direction vector

            if (toAsteroid.sqrMagnitude <= maxRangeSqr)     //This checks the range 
            {
                Vector3 end = origin + toAsteroid.normalized * lineLength;      //This calculates the line endpoint, this is supposed to end at 2.5f away from the player in the direction of the asteroid.
                Debug.DrawLine(origin, end, Color.green, 0f);       //Draw the line
            }
        }
    }
}
