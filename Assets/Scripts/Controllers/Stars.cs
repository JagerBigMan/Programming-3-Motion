using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime = 2f;
    public Color lineColor = Color.white;

    private int currentIndex = 0;
    private float segmentTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (starTransforms == null || starTransforms.Count < 2) return;

        int nextIndex = (currentIndex + 1) % starTransforms.Count;

        Vector3 a = starTransforms[currentIndex].position;
        Vector3 b = starTransforms[nextIndex].position;

        float progress = (drawingTime <= 0f) ? 1f : Mathf.Clamp01(segmentTimer / drawingTime);

        Vector3 p = Vector3.Lerp(a, b, progress);

        Debug.DrawLine(a, p, lineColor, 0f);

        segmentTimer += Time.deltaTime;
        if (segmentTimer >= drawingTime)
        {
            AdvanceSegment();
        }
    }
    private void AdvanceSegment()
    {
        segmentTimer = 0f;
        currentIndex = (currentIndex + 1) % starTransforms.Count;
    }
}
