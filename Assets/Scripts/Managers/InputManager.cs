using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private UIManager uiManager;
    public LayerMask cellLayer;
    [SerializeField] private Camera mainCamera;

    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.actions.FindAction("Gameplay/Click");
        pointAction = playerInput.actions.FindAction("Gameplay/Point");
    }

    public void Initialize(UIManager uiManager) { this.uiManager = uiManager; }

    void Update()
    {
        if (clickAction == null || pointAction == null || uiManager == null || mainCamera == null)
        {
            return;
        }

        if (clickAction.WasPressedThisFrame())
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector2 screenPosition = pointAction.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, cellLayer))
            {
                Cell clickedCell = hit.collider.GetComponent<Cell>();

                if (clickedCell != null && !clickedCell.IsOccupied && clickedCell.IsPlaceable)
                {
                    uiManager.ShowPlacementPopup(clickedCell);
                }
                else
                {
                    uiManager.HidePlacementPopup();
                }
            }
            else
            {
                uiManager.HidePlacementPopup();
            }
        }
    }
}