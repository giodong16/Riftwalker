using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance; 
    [SerializeField]private PlayerInput playerInput;

    private InputAction _moveAction;
    public InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _slidingAction;

    public Vector2 Vector2Move { get; private set; } 
    public bool JumpPressed { get; set; }
    public bool JumpReleased {  get; private set; }
    public bool DashPressed {  get; private set; }
    public bool SlidePressed {  get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerInput = GetComponent<PlayerInput>();
        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
        _dashAction = playerInput.actions["Dash"];
        _slidingAction = playerInput.actions["Sliding"];
    }

    private void Update()
    {
        // Đọc giá trị di chuyển
        Vector2Move = _moveAction.ReadValue<Vector2>();


    /*    JumpPressed = _jumpAction.triggered; 
      //  JumpReleased = _jumpAction.phase == InputActionPhase.Canceled; // đang không bắt được
        DashPressed = _dashAction.triggered;
        SlidePressed = _slidingAction.triggered;*/
    }
}
