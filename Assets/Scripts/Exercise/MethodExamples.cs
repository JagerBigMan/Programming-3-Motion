using UnityEngine;

public class MethodExamples : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DrawBoxAtPosition(mousePos, Vector2.one, new Color(1, 1, 1, 0.5f));
    }

    void DrawBoxAtPosition(Vector2 position, Vector2 size, Color color)
    {
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        Vector2 topLeft = new Vector2(position.x - halfWidth, position.y + halfHeight);
        Vector2 topRight = topLeft + Vector2.right * size.x;
        Vector2 bottomRight = topRight + Vector2.down * size.y;
        Vector2 bottomLeft = bottomRight + Vector2.left * size.x;

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);

    }
}
