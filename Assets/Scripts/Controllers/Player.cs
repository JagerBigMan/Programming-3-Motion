using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public Vector2 bombOffset = new Vector2(0, 1);      //Single bomb spawning by pressing b

    public float bombTrailSpacing = 1f;     //Distance between the trail bombs
    public int numberOfTrailBombs = 3;      //How many bombs to spawn in the trail

    public float randomCornerDistance = 2f;     //Distance from player to each corner

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
}
