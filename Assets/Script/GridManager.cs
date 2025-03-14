using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject[] candyPrefabs; // Gán 5 prefab kẹo vào đây trong Inspector
    public int gridWidth = 5; // Lưới 5x5 để đơn giản lúc đầu
    public int gridHeight = 5;
    public float tileSize = 1f; // Khoảng cách giữa các ô
    private GameObject[,] gridArray; // Lưu trữ kẹo trong lưới

    void Start()
    {
        gridArray = new GameObject[gridWidth, gridHeight];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject candy = Instantiate(candyPrefabs[Random.Range(0, candyPrefabs.Length)],
                    new Vector3(x * tileSize - (gridWidth / 2f), y * tileSize - (gridHeight / 2f), 0),
                    Quaternion.identity);
                candy.transform.parent = transform;
                gridArray[x, y] = candy;

                // Gán vị trí cho Candy script
                Candy candyScript = candy.GetComponent<Candy>();
                candyScript.xIndex = x;
                candyScript.yIndex = y;
            }
        }
    }
    private Candy selectedCandy = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Candy candy = hit.collider.GetComponent<Candy>();
                if (candy != null)
                {
                    if (selectedCandy == null)
                    {
                        selectedCandy = candy;
                    }
                    else
                    {
                        if (AreAdjacent(selectedCandy, candy))
                        {
                            SwapCandies(selectedCandy, candy);
                            selectedCandy = null;
                        }
                        else
                        {
                            selectedCandy.transform.localScale = new Vector3(1f, 1f, 1f);
                            selectedCandy = candy;
                        }
                    }
                }
            }
        }
    }

    bool AreAdjacent(Candy candy1, Candy candy2)
    {
        return (Mathf.Abs(candy1.xIndex - candy2.xIndex) == 1 && candy1.yIndex == candy2.yIndex) ||
               (Mathf.Abs(candy1.yIndex - candy2.yIndex) == 1 && candy1.xIndex == candy2.xIndex);
    }

    void SwapCandies(Candy candy1, Candy candy2)
    {
        int x1 = candy1.xIndex, y1 = candy1.yIndex;
        int x2 = candy2.xIndex, y2 = candy2.yIndex;

        gridArray[x1, y1] = candy2.gameObject;
        gridArray[x2, y2] = candy1.gameObject;

        candy1.xIndex = x2; candy1.yIndex = y2;
        candy2.xIndex = x1; candy2.yIndex = y1;

        Vector3 tempPos = candy1.transform.position;
        candy1.transform.position = candy2.transform.position;
        candy2.transform.position = tempPos;

        candy1.transform.localScale = new Vector3(1f, 1f, 1f);
        candy2.transform.localScale = new Vector3(1f, 1f, 1f);

        CheckMatches();
    }
    void CheckMatches()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (x < gridWidth - 2 && gridArray[x, y] != null && gridArray[x + 1, y] != null && gridArray[x + 2, y] != null)
                {
                    if (gridArray[x, y].tag == gridArray[x + 1, y].tag && gridArray[x, y].tag == gridArray[x + 2, y].tag)
                    {
                        Destroy(gridArray[x, y]);
                        Destroy(gridArray[x + 1, y]);
                        Destroy(gridArray[x + 2, y]);
                    }
                }
                if (y < gridHeight - 2 && gridArray[x, y] != null && gridArray[x, y + 1] != null && gridArray[x, y + 2] != null)
                {
                    if (gridArray[x, y].tag == gridArray[x, y + 1].tag && gridArray[x, y].tag == gridArray[x, y + 2].tag)
                    {
                        Destroy(gridArray[x, y]);
                        Destroy(gridArray[x, y + 1]);
                        Destroy(gridArray[x, y + 2]);
                    }
                }
            }
        }
    }
}