using UnityEngine;

public class GridManager : MonoBehaviour
{
    private LevelData levelData;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float padding = 0.2f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform gridContainer;

    [SerializeField] private Transform groundTransform;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraPadding = 1f;

    public void Initialize(LevelData data)
    {
        this.levelData = data;
        GenerateGrid();
        AdjustCamera();
    }

    private void GenerateGrid()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < levelData.gridWidth; x++)
        {
            for (int y = 0; y < levelData.gridHeight; y++)
            {
                Vector3 cellPosition = new Vector3(x * (cellSize + padding), 0.1f, y * (cellSize + padding));
                GameObject newCellObject = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                newCellObject.transform.SetParent(gridContainer);
                newCellObject.name = $"Cell_{x}_{y}";

                Cell cellScript = newCellObject.GetComponent<Cell>();
                if (cellScript != null)
                {
                    bool isCellPlaceable = y >= levelData.placeableMinRow && y <= levelData.placeableMaxRow;
                    cellScript.Initialize(x, y, isCellPlaceable);
                }
            }

        }
    }

    private void AdjustCamera()
    {
        float totalCellSize = cellSize + padding;
        float gridWorldWidth = levelData.gridWidth * totalCellSize;
        float gridWorldHeight = levelData.gridHeight * totalCellSize;

        float cameraSizeForHeight = gridWorldHeight / 2f;
        float cameraSizeForWidth = (gridWorldWidth / mainCamera.aspect) / 2f;

        mainCamera.orthographicSize = Mathf.Max(cameraSizeForHeight, cameraSizeForWidth) * cameraPadding;

        float gridCenterX = (gridWorldWidth / 2f) - (totalCellSize / 2f);
        float gridCenterZ = (gridWorldHeight / 2f) - (totalCellSize / 2f);

        Vector3 gridCenter = new Vector3(gridCenterX, 0, gridCenterZ);

        if (groundTransform != null)
        {
            groundTransform.position = gridCenter;

            float groundScaleX = (gridWorldWidth * cameraPadding * 2) / 10f;
            float groundScaleZ = (gridWorldHeight * cameraPadding * 2) / 10f;
            groundTransform.localScale = new Vector3(groundScaleX, 1f, groundScaleZ);
        }

        mainCamera.transform.position = new Vector3(gridCenterX, mainCamera.transform.position.y, gridCenterZ);
    }

    public Vector3[] GetSpawnPositions()
    {
        int gridWidth = levelData.gridWidth;
        // int topRowY = levelData.gridHeight - 1;
        int topRowY = levelData.gridHeight; // Spawn one cell above

        Vector3[] positions = new Vector3[gridWidth];

        for (int x = 0; x < gridWidth; x++)
        {
            Vector3 localPosition = new Vector3(x * (cellSize + padding), 0.1f, topRowY * (cellSize + padding));

            positions[x] = gridContainer.TransformPoint(localPosition);
        }

        return positions;
    }

    public float GetCellSize()
    {
        return cellSize + padding;
    }
    
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        float totalCellSize = cellSize + padding;

        int x = Mathf.RoundToInt(worldPosition.x / totalCellSize);
        int y = Mathf.RoundToInt(worldPosition.z / totalCellSize);

        return new Vector2Int(x, y);
    }
}
