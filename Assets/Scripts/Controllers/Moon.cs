using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform target; 
    public float radius = 5f;
    public float orbitalSpeed = 30f;

    private float angle;      
    
    public Transform planetTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) return;

        transform.position = target.position + new Vector3(radius, 0f, 0f);
        angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        angle += orbitalSpeed * Mathf.Deg2Rad * Time.deltaTime;

        if (angle > Mathf.PI * 2f)
            angle -= Mathf.PI * 2f;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = target.position + new Vector3(x, y, 0f);
    }
}
