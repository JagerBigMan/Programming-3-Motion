using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float arrivalDistance = 0.5f;
    public float maxFloatDistance = 5f;

    private Vector3 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        ChooseNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, transform.position) < arrivalDistance)
        {
            ChooseNewTarget();
        }    
    }
    private void ChooseNewTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 offset = new Vector3(randomDirection.x, randomDirection.y, 0f) * maxFloatDistance;
    }
}
