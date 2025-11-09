using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject asteroid in asteroids)
        {
            Collider2D asteroidCollider = asteroid.GetComponent<Collider2D>();
            if (asteroidCollider == null) continue;

            foreach (GameObject player in players)
            {
                Collider2D playerCollider = player.GetComponent<Collider2D>();
                if(playerCollider == null) continue;

                Physics2D.IgnoreCollision(asteroidCollider, playerCollider);    //Disabled collision between asteroids and player

                bool isIgnored = Physics2D.GetIgnoreCollision(asteroidCollider, playerCollider);    //Added a check to confirm whether the ignore was applied

                if (isIgnored)
                    Debug.Log("Collision ignored confirmed");
                else
                    Debug.LogWarning("Collision ignored failed!");
            }
        }
    }
}
