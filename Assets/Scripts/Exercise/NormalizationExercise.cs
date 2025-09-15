using UnityEngine;

public class NormalizationExercise : MonoBehaviour
{

    void Start()
    {

        Debug.Log(NormalizeVector(new Vector2(3,4)));
        Debug.Log(NormalizeVector(new Vector2(-3,2)));
        Debug.Log(NormalizeVector(new Vector2(1.5f, -3.5f)));
    }

    // Update is called once per frame
    void Update()
    {

    }
    Vector2 NormalizeVector(Vector2 vec)
    {
        Vector2 normalized;

        float magnitude = vec.magnitude;
        normalized = new Vector2(vec.x / magnitude, vec.y / magnitude);

        return normalized;
    }
}
