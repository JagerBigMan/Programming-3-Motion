using UnityEngine;

public class SquareSpawner : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            DrawBoxAtPosition(mousePos, Vector2.one, Color.white);                          //Using examples from week 2 in class exercises

        }
    }
        void DrawBoxAtPosition(Vector2 position, Vector2 size, Color color)
    {
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        Vector2 topLeft = new Vector2(position.x - halfWidth, position.y + halfHeight);
        Vector2 topRight = topLeft + Vector2.right * size.x;
        Vector2 bottomRight = topRight + Vector2.down * size.y;
        Vector2 bottomLeft = bottomRight + Vector2.left * size.x;

        Debug.DrawLine(topLeft, topRight, color, 1f);
        Debug.DrawLine(topRight, bottomRight, color, 1f);
        Debug.DrawLine(bottomRight, bottomLeft, color, 1f);
        Debug.DrawLine(bottomLeft, topLeft, color, 1f);
    }
}
