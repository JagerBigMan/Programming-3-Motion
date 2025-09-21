using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))        
        {
            SpawnBombAtOffset(new Vector3(0, 1));  //Same as Vector3.up since it's 0,1, got it from Jeff in class
        }
    }

    private void SpawnBombAtOffset(Vector3 inOffset)
        {
            Instantiate(bombPrefab, transform.position + inOffset, Quaternion.identity);        //Got it in class
        }
}
