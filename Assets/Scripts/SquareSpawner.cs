using UnityEngine;

public class SquareSpawner : MonoBehaviour
{
    private Vector2 boxSize = Vector2.one;
    private Color previewColor = new Color(1f, 1f, 1f, 0.3f);   //I'm calling the Semi-transparent square the preview square because that's what it reminded me of

    private float sizePerScrollTick = 0.1f;     //I want to make each tick influence the size smaller so I'm using 0.1
    private float minSize = 0.1f;       //This so that the square doesn't disappear

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > Mathf.Epsilon)      
        {
            float changedSize = Mathf.Max(minSize, boxSize.x + scroll * sizePerScrollTick);
            boxSize = new Vector2(changedSize, changedSize);        //This changes the size of x and y equally.
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            DrawBoxAtPosition(mousePos, boxSize, Color.white);                          //Using examples from week 2 in class exercises

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
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = previewColor;
        Gizmos.DrawCube(mousePos, new Vector3(boxSize.x, boxSize.y, 0.01f));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(mousePos, new Vector3(boxSize.x, boxSize.y, 0.01f));
    }
}
