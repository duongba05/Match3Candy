using UnityEngine;

public class Candy : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    private GridManager gridManager;
    private bool isSelected = false;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    void OnMouseDown()
    {
        if (!isSelected)
        {
            isSelected = true;
            transform.localScale = new Vector3(1.2f, 1.2f, 1f); // Phóng to khi chọn
        }
        else
        {
            isSelected = false;
            transform.localScale = new Vector3(1f, 1f, 1f); // Trở lại bình thường
        }
    }
}