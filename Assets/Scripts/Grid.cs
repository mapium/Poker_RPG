using UnityEngine;
using System.Collections;

public class TestGrid : MonoBehaviour
{
    private Grid grid;
    private void Awake()
    {
        grid = this.GetComponent<Grid>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = grid.WorldToCell(mousePos);

            Debug.Log($" лик по €чейке {cellPos}");
        }
    }
}