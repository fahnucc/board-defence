using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private Material nonPlaceableMaterial;
    [SerializeField] private Material selectedMaterial;
    private int x, y;
    private bool isEndZone = false;

    private Renderer cellRenderer;
    private GameObject placedItem = null;
    public bool IsOccupied => placedItem != null;
    public bool IsPlaceable { get; private set; }

    void Awake()
    {
        cellRenderer = GetComponent<Renderer>();
    }

    public void Initialize(int x, int y, bool isPlaceable)
    {
        this.x = x;
        this.y = y;
        this.IsPlaceable = isPlaceable;
        this.isEndZone = y == 0;
        UpdateVisuals();
    }

    public void UpdateVisuals(bool selected = false)
    {
        if (cellRenderer != null)
        {
            if (selected) cellRenderer.material = selectedMaterial;
            else cellRenderer.material = IsPlaceable ? placeableMaterial : nonPlaceableMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Debug.Log($"trigger enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (isEndZone)
            {
                // Debug.LogWarning("game over");

                Destroy(other.gameObject);
                // end game
                GameManager.Instance.GameOver();
            }
        }
    }

    public void PlaceItem(GameObject itemPrefab, DefenceItemData defenceItemData)
    {
        if (IsOccupied) return;

        placedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        DefenceItemController controller = placedItem.GetComponent<DefenceItemController>();
        controller.Initialize(defenceItemData, x, y);
    }
}