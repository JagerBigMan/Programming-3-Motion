using UnityEngine;
using UnityEngine.UI;


public class RowGeneration : MonoBehaviour
{
    public Button generateButton;
    public InputField squareNumberInput;

    public GameObject squarePrefab;
    public Transform spawnParent;

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
            Debug.Log("Generating" + numberOfSquares + "squares");

            for (int i = 0; i < numberOfSquares; i++)
            {
                Vector3 spawnPos = new Vector3(i * 1.2f, 0f, 0f);
                GameObject square = Instantiate(squarePrefab, spawnPos, Quaternion.identity);

                if (spawnParent != null)
                {
                    square.transform.SetParent(spawnParent, false);
                }
            }

        }
        else
        {
            Debug.LogWarning("Invalid input! Please enter a number");
        }
    }
}
