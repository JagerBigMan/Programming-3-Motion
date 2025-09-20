using UnityEngine;
using UnityEngine.UI;


public class RowGeneration : MonoBehaviour
{
    public Button generateButton;
    public InputField squareNumberInput;

    public float squareSize = 1f;
    public float spacing = 0.2f;
    public float drawDuration = 5f;

    void Start()
    {
        generateButton.onClick.AddListener(GenerateRow);
    }

    void GenerateRow()
    {
        string inputText = squareNumberInput.text;

        int numberOfSquares;
        if (int.TryParse(inputText, out numberOfSquares))
        {
            Debug.Log("Drawing" + numberOfSquares + "squares");

            for (int i = 0; i < numberOfSquares; i++)
            {
                Vector3 startPos = new Vector3(i * (squareSize + spacing), 0f, 0f);

                Vector3 bottomLeft = startPos;
                Vector3 bottomRight = startPos + new Vector3(squareSize, 0f, 0f);
                Vector3 topRight = startPos + new Vector3(squareSize, squareSize, 0f);
                Vector3 topLeft = startPos + new Vector3(0f, squareSize, 0f);

                Debug.DrawLine(bottomLeft, bottomRight, Color.green, drawDuration);
                Debug.DrawLine(bottomRight, topRight, Color.green, drawDuration);
                Debug.DrawLine(topRight, topLeft, Color.green, drawDuration);
                Debug.DrawLine(topLeft, bottomLeft, Color.green, drawDuration);
            }

        }
        else
        {
            Debug.LogWarning("Invalid input! Please enter a number");
        }
    }
}
