using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private UIManager uiManager;
    public LayerMask cellLayer;

    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.actions["Click"];
    }

    public void Initialize(UIManager uiManager) { this.uiManager = uiManager; }

    void Update()
    {
        if (clickAction.WasPressedThisFrame())
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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